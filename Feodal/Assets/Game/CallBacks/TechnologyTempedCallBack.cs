using Game.DataStructures.Technologies.Abstraction;

namespace Game.CallBacks
{
    public class TechnologyTempedCallBack
    {
        public string Title;
        public ITechnologyStore Technology;
        public bool Value;
        public TechnologyTempedCallBack(string title, ITechnologyStore technology, bool value)
        {
            Title = title;
            Technology = technology;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Title}{Technology.ToString()}{Value}";
        }
    }
}