using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace Loggly.Tests
{
   public abstract class BaseFixture
   {
      protected static readonly Func<IDictionary<string, object>> EmptyPayload = () => new Dictionary<string, object>();

      protected FakeServer Server;
      protected AutoResetEvent Trigger;

      protected virtual bool NeedAServer
      {
         get { return true; }
      }

      [SetUp]
      public void SetUp()
      {
         Trigger = new AutoResetEvent(false);
         if (NeedAServer)
         {
            Server = new FakeServer();
            LogglyConfiguration.Configure(c => c.ForceUrlTo("http://localhost:" + FakeServer.Port + "/"));
         }
         BeforeEachTest();
      }

      [TearDown]
      public void TearDown()
      {
         if (Server != null)
         {
            Server.Dispose();
            LogglyConfiguration.ResetToDefaults();
         }
         AfterEachTest();
      }

      public virtual void AfterEachTest()
      {
      }

      public virtual void BeforeEachTest()
      {
      }


      protected void Set()
      {
         Trigger.Set();
      }

      protected void WaitOne()
      {
         Assert.IsTrue(Trigger.WaitOne(6000), "Test terminated without properly signalling the trigger");
      }
   }

   internal static class AutoResetEventExtensions
   {
      public static void Wait(this AutoResetEvent trigger, int count)
      {
         for (var i = 0; i < count; ++i)
         {
            trigger.WaitOne();
         }
      }
   }
}