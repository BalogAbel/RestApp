﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RestApp.ReservationService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ReservationService.IReservationService")]
    public interface IReservationService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReservationService/DoWork", ReplyAction="http://tempuri.org/IReservationService/DoWorkResponse")]
        void DoWork();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReservationService/DoWork", ReplyAction="http://tempuri.org/IReservationService/DoWorkResponse")]
        System.Threading.Tasks.Task DoWorkAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IReservationServiceChannel : RestApp.ReservationService.IReservationService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ReservationServiceClient : System.ServiceModel.ClientBase<RestApp.ReservationService.IReservationService>, RestApp.ReservationService.IReservationService {
        
        public ReservationServiceClient() {
        }
        
        public ReservationServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ReservationServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReservationServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReservationServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void DoWork() {
            base.Channel.DoWork();
        }
        
        public System.Threading.Tasks.Task DoWorkAsync() {
            return base.Channel.DoWorkAsync();
        }
    }
}