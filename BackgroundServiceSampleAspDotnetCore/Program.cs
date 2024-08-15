// See https://aka.ms/new-console-template for more information
using BackgroundServiceSampleAspDotnetCore;

Console.WriteLine("Hello, World!");

//------------------------------------------------------------------------------
//---------------------------------Timer Sample---------------------------------
//------------------------------------------------------------------------------

//var sampleTimer = new ClassSampleTimer(5);
//var autoResetEvent = new AutoResetEvent(false);

//var timerSample = new Timer(sampleTimer.TestMethod, autoResetEvent, 2000, 1000);

//autoResetEvent.WaitOne();

//timerSample.Change(0, 500);
//autoResetEvent.WaitOne();

//timerSample.Dispose();
//autoResetEvent.Dispose();

//------------------------------------------------------------------------------
//---------------------------------Timer Sample---------------------------------
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//--------------------------------Channel Sample--------------------------------
//------------------------------------------------------------------------------

//ClassSampleQueue myQueue = new ClassSampleQueue();
//ClassSampleProducer myProducer = new ClassSampleProducer(myQueue._channel);
//ClassSampleCunsomer myCunsomer= new ClassSampleCunsomer(myQueue._channel);

//await Task.WhenAll(myProducer.SendMessage() , myCunsomer.GetMessage());

//------------------------------------------------------------------------------
//--------------------------------Channel Sample--------------------------------
//------------------------------------------------------------------------------