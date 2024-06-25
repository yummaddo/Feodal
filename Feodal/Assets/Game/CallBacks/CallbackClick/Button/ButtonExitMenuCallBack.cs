﻿using Game.CallBacks.CallbackClick.Abstraction;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers;
using Game.UI.Menu;

namespace Game.CallBacks.CallbackClick.Button
{
    public class ButtonExitMenuCallBack: ButtonClickCallback<MenuTypes>
    {
        public override void Initialization()
        {
            StatusInit = true;
            MenuTypesExitProvider.CallBackTunneling<ButtonExitMenuCallBack>(this);
        }

        protected override void OnButtonClick()
        {
            
        }

        public override Port GetPort()
        {
            return Porting.Type<ButtonExitMenuCallBack>();
        }
    }
}