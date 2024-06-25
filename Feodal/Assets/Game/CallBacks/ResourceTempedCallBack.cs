using Game.DataStructures.Abstraction;

namespace Game.CallBacks
{
    public class ResourceTempedCallBack
    {
        public string Title;
        public IResource Resource;
        public long Value;
        public ResourceTempedCallBack(string title, IResource resource, long value)
        {
            Title = title;
            Resource = resource;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Title}{Resource.ToString()}{Value}";
        }
    }
}