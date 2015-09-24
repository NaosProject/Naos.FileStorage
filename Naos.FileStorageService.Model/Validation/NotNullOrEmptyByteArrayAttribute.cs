namespace Naos.FileStorage.Contract.Validation
{
    using System.ComponentModel.DataAnnotations;

    public class NotNullOrEmptyByteArrayAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var byteArray = value as byte[];

            if (byteArray == null || byteArray.Length == 0)
            {
                return false;
            }

            return true;
        }
    }
}