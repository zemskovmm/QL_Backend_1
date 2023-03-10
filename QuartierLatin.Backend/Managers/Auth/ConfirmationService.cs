using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Nito.AsyncEx;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Config;
using QuartierLatin.Backend.ViewModels;

namespace QuartierLatin.Backend.Managers.Auth
{
    public interface IConfirmationService
    {
        public Task SendNewConfirmationCode(int userId);
        public Task ConfirmUser(string code);
    }

    public interface IEmailConfirmationService : IConfirmationService
    {
    }

    public class NoopEmailConfirmationService : IEmailConfirmationService
    {
        private readonly IAdminRepository _userRepository;

        public NoopEmailConfirmationService(IAdminRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task SendNewConfirmationCode(int userId)
        {
            var user = _userRepository.GetById(userId);
            user.Confirmed = true;
            _userRepository.Update(user);
            await Task.Yield();
        }

        public async Task ConfirmUser(string code)
        {
            var user = _userRepository.FindByConfirmCode(code);
            if (user is null) throw new ArgumentException("Code mismatch");

            user.Confirmed = true;
            user.ConfirmationCode = null;
            _userRepository.Update(user);
            await Task.Yield();
        }
    }

    public class EmailConfirmationService : IEmailConfirmationService, IDisposable
    {
        private readonly AsyncLock _asyncLock = new AsyncLock();
        private readonly IAdminRepository _userRepository;
        private readonly IRazorRenderer _razorRenderer;
        private readonly SmtpClient _smtpClient = new SmtpClient();
        private readonly EmailOptions _options;

        public EmailConfirmationService(IAdminRepository userRepository, IRazorRenderer razorRenderer,
            IOptions<EmailOptions> options)
        {
            _userRepository = userRepository;
            _razorRenderer = razorRenderer;
            _options = options.Value;
        }

        private async Task Connect()
        {
            _smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
            await _smtpClient.ConnectAsync(_options.SMTP.Host, _options.SMTP.Port);
            if (!_options.SMTP.Auth) return;

            _smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
            await _smtpClient.AuthenticateAsync(_options.SMTP.Username, _options.SMTP.Password);
        }

        public async Task SendNewConfirmationCode(int userId)
        {
            using (await _asyncLock.LockAsync())
            {
                await Connect();
                var user = _userRepository.GetById(userId);
                var confirmCode = Guid.NewGuid().ToString().Replace("-", "");
                var model = new EmailConfirmTemplateViewModel
                {
                    ConfirmationsCode = confirmCode,
                    RedirectUrl = $"{_options.BaseUrl}/confirm"
                };
                var text = await _razorRenderer.RenderViewToStringAsync("/Views/EmailConfirmTemplate.cshtml", model);
                var body = new BodyBuilder {HtmlBody = text};
                var message = new MimeMessage
                {
                    From = {new MailboxAddress(_options.Name, _options.FromEmail)},
                    To = {new MailboxAddress(user.Name, user.Email)},
                    Subject = "?????????????????????????? ??????????????????????",
                    Body = body.ToMessageBody()
                };

                await _smtpClient.SendAsync(message);
                await _smtpClient.DisconnectAsync(false);
                user.ConfirmationCode = confirmCode;
                _userRepository.Update(user);
            }
        }

        public async Task ConfirmUser(string code)
        {
            var user = _userRepository.FindByConfirmCode(code);
            if (user is null) throw new ArgumentException("Code mismatch");

            user.Confirmed = true;
            user.ConfirmationCode = null;
            _userRepository.Update(user);
            await Task.Yield();
        }

        public void Dispose()
        {
            _smtpClient.Disconnect(true);
            _smtpClient.Dispose();
        }
    }
}