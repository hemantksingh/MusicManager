﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace Test.MusicManager
{
    [Specification]
    public abstract class BaseTestFixture
    {
        protected Exception CaughtException;
        protected virtual void Given() { }
        protected abstract void When();
        protected virtual void Finally() { }

        [Given]
        public void Setup()
        {
            CaughtException = new ThereWasNoExceptionButOneWasExpectedException();            
            Given();

            try
            {
                When();
            }
            catch (Exception exception)
            {
                CaughtException = exception;
            }
            finally
            {
                Finally();
            }
        }
    }

    public abstract class BaseTestFixture<TSubjectUnderTest>
    {
        private Dictionary<Type, object> _mocks;

        protected Dictionary<Type, object> DoNotMock;
        protected TSubjectUnderTest SubjectUnderTest;
        protected Exception CaughtException;
        protected virtual void SetupDependencies() { }
        protected virtual void Given() { }
        protected abstract void When();
        protected virtual void Finally() { }

        [Given]
        public void Setup()
        {
            _mocks = new Dictionary<Type, object>();
            DoNotMock = new Dictionary<Type, object>();
            CaughtException = new ThereWasNoExceptionButOneWasExpectedException();

            BuildMocks();
            SetupDependencies();
            SubjectUnderTest = BuildSubjectUnderTest();

            Given();

            try
            {
                When();
            }
            catch (Exception exception)
            {
                CaughtException = exception;
            }
            finally
            {
                Finally();
            }
        }

        public Mock<TType> OnDependency<TType>() where TType : class
        {
            return (Mock<TType>)_mocks[typeof(TType)];
        }

        private TSubjectUnderTest BuildSubjectUnderTest()
        {
            var constructorInfo = typeof(TSubjectUnderTest).GetConstructors().First();

            var parameters = new List<object>();
            foreach (var mock in _mocks)
            {
                object theObject;
                if (!DoNotMock.TryGetValue(mock.Key, out theObject))
                    theObject = ((Mock)mock.Value).Object;

                parameters.Add(theObject);
            }

            return (TSubjectUnderTest)constructorInfo.Invoke(parameters.ToArray());
        }

        private void BuildMocks()
        {
            var constructorInfo = typeof(TSubjectUnderTest).GetConstructors().First();

            foreach (var parameter in constructorInfo.GetParameters())
            {
                _mocks.Add(parameter.ParameterType, CreateMock(parameter.ParameterType));
            }
        }

        private static object CreateMock(Type type)
        {
            var constructorInfo = typeof(Mock<>).MakeGenericType(type).GetConstructors().First();
            return constructorInfo.Invoke(new object[] { });
        }
    }

    public class GivenAttribute : SetUpAttribute { }

    public class ThenAttribute : TestAttribute { }

    public class SpecificationAttribute : TestFixtureAttribute { }
}

