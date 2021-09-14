using System;
using System.Threading.Tasks;
using CommandLine;
using QuartierLatin.Backend.Managers.Auth;

namespace QuartierLatin.Backend.Cmdlets
{
    public class EncodePasswordCmdletBase : CmdletBase<EncodePasswordCmdletBase.EncodePassOptions>
    {
        [Verb("encodepass")]
        public class EncodePassOptions
        {
            
            [Value(0)]
            public string Password { get; set; }
        }
        
        protected override Task<int> Execute(EncodePassOptions args)
        {
            if (args.Password == null)
            {
                Console.WriteLine("Enter:");
                args.Password = Console.ReadLine();
            }
            
            Console.WriteLine(PasswordToolkit.EncodeSshaPassword(args.Password));
            return Task.FromResult(0);
        }
    }
}