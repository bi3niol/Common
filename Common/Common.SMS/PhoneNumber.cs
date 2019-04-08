namespace Common.SMS
{
    public class PhoneNumber
    {
        public string Input { get; set; }
        public string CountryCode { get; }
        public string Number { get=>Input; }
        public override string ToString()
        {
            return Number;
        }
    }
}