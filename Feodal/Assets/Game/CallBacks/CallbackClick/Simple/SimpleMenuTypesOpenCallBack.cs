using Game.CallBacks.CallbackClick.Abstraction;
using Game.UI.Menu;

namespace Game.CallBacks.CallbackClick.Simple
{
    public class SimpleMenuTypesOpenCallBack : SimpleClickCallback<MenuTypes>
    {
        public override void Initialization()
        {
            // MenuTypesOpenProvider.CallBackTunneling(Port.Port2,this);
        }
    }
}