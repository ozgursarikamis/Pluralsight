﻿using System;
using System.Collections.Generic;
using AutoFixture;
using Xunit;

namespace Fixture.Tests
{
    public class SequenceDemos
    {
        [Fact]
        public void SequenceOfStrings()
        {
            var fixture = new AutoFixture.Fixture();

            IEnumerable<string> messages = fixture.CreateMany<string>(6);

        }

        [Fact]
        public void AddingToExistingList()
        {
            var fixture = new AutoFixture.Fixture();
            var sut = new DebugMessageBuffer();

            fixture.AddManyTo(sut.Messages);
            sut.WriteMessages();
            Assert.Equal(3, sut.MessagesWritten);
        }

        [Fact]
        public void ExplicitNumbersOfItems()
        {
            var fixture = new AutoFixture.Fixture();
            IEnumerable<int> numbers = fixture.CreateMany<int>(6);

        }
        [Fact]
        public void AddingToExistingListWithCreator()
        {
            var fixture = new AutoFixture.Fixture();
            var sut = new DebugMessageBuffer();

            var rnd = new Random();

            fixture.AddManyTo(sut.Messages, () => rnd.Next().ToString());
        } 
    }
}