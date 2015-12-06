using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using RestApp.Login;

namespace RestApp
{
    public class AppBootstrapper : BootstrapperBase
    {
        private CompositionContainer container;

        public AppBootstrapper()
        {
            Initialize();
        }


        protected override void Configure()
        {
            container = new CompositionContainer(
                new AggregateCatalog(
                    AssemblySource.Instance.Select(
                        x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()
                    )
                );

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(container);

            container.Compose(batch);
        }

        protected override object GetInstance(Type service, string key)
        {
            var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(service) : key;
            var exports = container.GetExportedValues<object>(contract);

            if (exports.Any())
            {
                return exports.First();
            }

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetExportedValues<object>(AttributedModelServices.GetContractName(service));
        }

        protected override void BuildUp(object instance)
        {
            container.SatisfyImportsOnce(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var windowManager = IoC.Get<IWindowManager>();
            Application.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            var vm = new LoginViewModel();
            windowManager.ShowDialog(vm);
            if (!vm.Success)
            {
                Application.Shutdown();
                return;
            }

            Application.ShutdownMode = ShutdownMode.OnLastWindowClose;
            DisplayRootViewFor<IShell>();
        }
    }
}