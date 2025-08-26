using AutoMapper;

namespace Back_Almazara.Utility
{
    public class RepositoryResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static RepositoryResult<T> Ok(T data, string? message = null)
            => new RepositoryResult<T> { Success = true, Data = data, Message = message };

        public static RepositoryResult<T> Fail(string message)
            => new RepositoryResult<T> { Success = false, Message = message, Data = default };

    }
    public static class RepositoryResultExtensions
    {
        public static RepositoryResult<TDestination> MapTo<TSource, TDestination>(
            this RepositoryResult<TSource> sourceResult, IMapper mapper)
        {
            return new RepositoryResult<TDestination>
            {
                Success = sourceResult.Success,
                Message = sourceResult.Message,
                Data = sourceResult.Data != null ? mapper.Map<TDestination>(sourceResult.Data) : default
            };
        }
    }

}
