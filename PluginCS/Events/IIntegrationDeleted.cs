using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginCS.Events
{
    public interface IIntegrationDeleted
    {
        void OnCalled(IntegrationDeleteEventArgs integrationDeleteEventArgs);
    }
}
