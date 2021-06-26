using AndYou.Views;
using SiraUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace AndYou.Installers
{
    class AndYouMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesTo<SettingViewController>().FromNewComponentAsViewController().AsSingle().NonLazy();
        }
    }
}
