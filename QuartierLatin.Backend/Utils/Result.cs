using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace QuartierLatin.Backend.Utils
{
    public class Result<T> : Result
    {
        public Result(T result)
        {
            Success = true;
            Value = result;
        }

        private Result()
        {
        }

        [JsonProperty] public T Value { get; }

        public Result<TRes> Map<TRes>(Func<T, TRes> cb)
        {
            if (Success)
                return Create(cb(Value));
            return Error;
        }

        public Result<TRes> Map<TRes>(Func<T, Result<TRes>> cb)
        {
            if (Success)
                return cb(Value);
            return Error;
        }

        public static implicit operator Result<T>(ErrorCode error)
        {
            return new()
            {
                Error = error
            };
        }

        public static implicit operator Result<T>(T value)
        {
            return new(value);
        }
    }

    public class Result
    {
        public bool Success { get; protected set; }

        public ErrorCode Error { get; protected set; }

        public static Result Succeeded { get; } = new() {Success = true};

        public static Result<T> Create<T>(T result)
        {
            return new(result);
        }

        public ErrorCode AsError()
        {
            return Error;
        }

        public static implicit operator Result(ErrorCode error)
        {
            return new() {Error = error};
        }

        public Result<TRes> Map<TRes>(Func<TRes> cb)
        {
            if (Success)
                return Create(cb());
            return Error;
        }

        public Result<TRes> Map<TRes>(Func<Result<TRes>> cb)
        {
            if (Success)
                return cb();
            return Error;
        }

        //Needed for early returns
        public bool IsError(out Result res)
        {
            res = this;
            return !Success;
        }

        public static Result Catch(Action cb, ErrorCode code)
        {
            try
            {
                cb();
                return Succeeded;
            }
            catch
            {
                return code;
            }
        }

        public static Result<T> Catch<T>(Func<T> cb, ErrorCode code, ILogger logger)
        {
            try
            {
                return cb();
            }
            catch (Exception e)
            {
                logger?.LogError(e, "Error");
                return code;
            }
        }

        public static Result<T> WhenNull<T>(T v, ErrorCode code) where T : class
        {
            if (v == null)
                return code;
            return v;
        }
    }

    public static class ResultExtensions
    {
        public static Result<T> WhenNull<T>(this T v, ErrorCode code) where T : class
        {
            return Result.WhenNull(v, code);
        }
    }

    public class ErrorCode
    {
        private ErrorCode(string code, string description)
        {
            Code = code;
            Description = description;
        }

        [JsonProperty] public string Code { get; }

        [JsonProperty] public string Description { get; }

        public static ErrorCode EmailIsAlreadyRegistered => D();
        public static ErrorCode NotFound => D();
        public static ErrorCode UserNotFound => D();
        public static ErrorCode InvalidPassword => D();
        public static ErrorCode DatabaseError => D();
        public static ErrorCode AccessDenied => D();
        public static ErrorCode InvalidState => D();
        public static ErrorCode InvalidEmail => D();
        public static ErrorCode WeakPassword => D();
        public static ErrorCode UserIsNotAssociatedWithVoter => D();
        public static ErrorCode UserIsNotInsideVotingDistrict => D();
        public static Result InvalidBallot => D();

        private static ErrorCode D(string description, [CallerMemberName] string code = null)
        {
            return new(code, description);
        }

        private static ErrorCode D([CallerMemberName] string code = null)
        {
            var sb = new StringBuilder();
            sb.Append(code[0]);
            foreach (var c in code.Skip(1))
                if (char.IsUpper(c))
                {
                    sb.Append(' ');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }

            return new ErrorCode(code, sb.ToString());
        }

        public override string ToString()
        {
            return Code + ": " + Description;
        }
    }
}