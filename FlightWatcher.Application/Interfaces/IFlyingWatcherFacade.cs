namespace FlightWatcher.Application.Interfaces
{
    public interface IFlyingWatcherFacade
    {
        string HowToUse();

        string WhatDoesItDo();

        void StartScan();
    }
}
