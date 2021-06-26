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
    class AndYouGameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesTo<AndYouScreenController>().FromNewComponentAsViewController().AsCached().NonLazy();
            
        }
    }
}
