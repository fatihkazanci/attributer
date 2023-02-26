namespace BusinessLibrary.Models
{
    public class BusinessResult<T>
    {
        public bool IsSuccess { get; set; } = false;
        public T? ReturnObject { get; set; }

        public void Success()
        {
            this.IsSuccess = true;
        }
        public void Success(T willReturnObject)
        {
            this.ReturnObject = willReturnObject;
            this.IsSuccess = true;
        }
    }
}
