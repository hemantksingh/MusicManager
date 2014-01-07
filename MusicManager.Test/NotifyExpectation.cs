﻿using System;
using System.ComponentModel;
using NUnit.Framework;

namespace MusicManager.Test
{
    public class NotifyExpectation<T> where T : INotifyPropertyChanged
    {
        private readonly T _owner;
        private readonly string _propertyName;
        private readonly bool _eventExpected;

        public NotifyExpectation(T owner, string propertyName, bool eventExpected)
        {
            _owner = owner;
            _propertyName = propertyName;
            _eventExpected = eventExpected;
        }

        public void When(Action<T> action)
        {
            bool eventWasRaised = false;
            _owner.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == _propertyName)
                {
                    eventWasRaised = true;
                }
            };
            action(_owner);

            Assert.AreEqual(_eventExpected, eventWasRaised,
                "PropertyChanged on {0}", this._propertyName);
        }

        public void When(Func<T, bool> action)
        {
            bool eventWasRaised = false;
            _owner.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == _propertyName)
                {
                    eventWasRaised = true;
                }
            };
            action(_owner);

            Assert.AreEqual(_eventExpected, eventWasRaised,
                "PropertyChanged on {0}", this._propertyName);
        }
    }
}
