using Autofac;
using SecurePhoneRetriever.Common;
using SecurePhoneRetriever.Model;
using SecurePhoneRetriever.Presenter;
using SecurePhoneRetriever.UI;
using System;
using System.Threading;
using System.Windows.Forms;

namespace SecurePhoneRetriever.Startup {
    internal static class Startup {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException +=
                new ThreadExceptionEventHandler(GlobalExceptionHandler);

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(GlobalExceptionHandler);

            var container = ConfigureContainer();

            Application.Run(container.Resolve<IViewGetter>()
                .View as Form);
        }

        private static IContainer ConfigureContainer() {
            var builder = new ContainerBuilder();

            builder
                .RegisterType<PNProcessingPresenter>()
                .As<IViewGetter>();

            builder
                .RegisterType<MainForm>()
                .As<IPNProcessorView>();

            builder
                .RegisterType<PNManager>()
                .As<IPNManager>();

            builder
                .RegisterType<PanelsToggle>()
                .As<IPanelsToggle>();

            builder
                .RegisterType<PNRepository>()
                .As<IPNRepository>();

            builder
                .RegisterType<PNValidator>()
                .As<IPNValidator>();

            builder
                .RegisterType<PNCoder>()
                .As<IPNCoder>();

            builder
                .RegisterType<AppSettings>()
                .As<IAppSettings>()
                .SingleInstance();

            return builder.Build();
        }

        private static void GlobalExceptionHandler(object sender, EventArgs e) {
            MessageBox.Show("Oops! There was a problem. Please contact us - ExcellentNonsense.feedback@gmail.com.");
            Application.Exit();
        }
    }
}
