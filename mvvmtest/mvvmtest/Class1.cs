using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Windows.Input;

namespace mvvmtest
{
    public class Class1: ObservableObject
    {
        private string name="ssss";

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        } 
    }
    public class MyViewModel : ObservableObject
    {
        public MyViewModel()
        {
            IncrementCounterCommand = new RelayCommand(IncrementCounter);
        }

        private int counter;

        public int Counter
        {
            get => counter;
            private set => SetProperty(ref counter, value);
        }

        public ICommand IncrementCounterCommand { get; }

        private void IncrementCounter() => Counter++;
    }
    
   
}
