using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace QuartierLatin.CityMapper
{
    public class AsyncEnumerableStream<T> : IAsyncEnumerable<T>, IAsyncDisposable
    {
        public class EnumerableReaderEnumerator<T> : IAsyncEnumerator<T>
        {

            private readonly Func<NpgsqlDataReader, T> _mapper;
            private readonly NpgsqlDataReader _dataReader;

            public EnumerableReaderEnumerator(Func<NpgsqlDataReader, T> mapper, NpgsqlDataReader dataReader)
            {
                this._mapper = mapper;
                this._dataReader = dataReader;
            }

            public async ValueTask DisposeAsync()
            {
                await _dataReader.DisposeAsync();
            }

            public async ValueTask<bool> MoveNextAsync() => await _dataReader.ReadAsync();

            public T Current => _mapper(_dataReader);
        }

        private readonly EnumerableReaderEnumerator<T> _enumerator;

        public AsyncEnumerableStream(Func<NpgsqlDataReader, T> mapper, NpgsqlDataReader dataReader)
        {
            _enumerator = new EnumerableReaderEnumerator<T>(mapper, dataReader);
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken()) 
            => _enumerator;

        public async ValueTask DisposeAsync()
        {
            await _enumerator.DisposeAsync();
        }
    }

    public static class AsyncEnumerableStreamExtensions
    {
        public static AsyncEnumerableStream<T> ToEnumerableStream<T>(this NpgsqlDataReader reader, Func<NpgsqlDataReader, T> func)
            => new(func, reader);
    }
}