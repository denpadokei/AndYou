using AndYou.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace AndYou.Installers
{
    class AndYouAppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<ChatManager>().AsSingle().NonLazy();
        }
    }
}
