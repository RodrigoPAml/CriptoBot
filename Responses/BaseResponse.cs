namespace CriptoBOT.Responses
{
    /// <summary>
    /// Represents the base response structure returned by the ByBit API.
    /// This class provides a standardized way to handle API responses.
    /// </summary>
    /// <typeparam name="T">The type of the result data returned in the response.</typeparam>
    public class BaseResponse<T>
    {
        /// <summary>
        /// Gets or sets the return code of the API response.
        /// A value of 0 typically indicates a successful operation, while non-zero indicates an error.
        /// </summary>
        public int RetCode { get; set; }

        /// <summary>
        /// Gets or sets the return message of the API response.
        /// This message provides additional information about the result, such as errors or status details.
        /// </summary>
        public string RetMsg { get; set; }

        /// <summary>
        /// Gets or sets the result data returned by the API.
        /// The result is of a generic type <typeparamref name="T"/>, enabling flexibility in response data structure.
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// Gets a value indicating whether the API response indicates success.
        /// The response is considered successful if <see cref="RetCode"/> equals 0.
        /// </summary>
        public bool Success => RetCode == 0;
    }
}
