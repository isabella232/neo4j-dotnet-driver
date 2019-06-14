// Copyright (c) 2002-2019 "Neo4j,"
// Neo4j Sweden AB [http://neo4j.com]
// 
// This file is part of Neo4j.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Linq;
using System.Reactive;
using FluentAssertions;
using Moq;
using Neo4j.Driver.Internal;
using Neo4j.Driver.Tests;
using Xunit;

namespace Neo4j.Driver.Reactive.Internal
{
    public static class InternalRxTransactionTests
    {
        public class Run : AbstractRxTest
        {
            [Fact]
            public void ShouldReturnInternalRxResult()
            {
                var rxTxc = new InternalRxTransaction(Mock.Of<ITransaction>());

                rxTxc.Run("RETURN 1").Should().BeOfType<InternalRxResult>();
            }

            [Fact]
            public void ShouldInvokeTxcRunAsyncOnKeys()
            {
                VerifyLazyRunAsync(r => r.Keys().SubscribeAndDiscard());
            }

            [Fact]
            public void ShouldInvokeTxcRunAsyncOnRecords()
            {
                VerifyLazyRunAsync(r => r.Records().SubscribeAndDiscard());
            }

            [Fact]
            public void ShouldInvokeTxcRunAsyncOnSummary()
            {
                VerifyLazyRunAsync(r => r.Summary().SubscribeAndDiscard());
            }

            [Fact]
            public void ShouldInvokeTxcRunAsyncOnlyOnce()
            {
                VerifyLazyRunAsync(r =>
                {
                    r.Keys().SubscribeAndDiscard();
                    r.Records().SubscribeAndDiscard();
                    r.Summary().SubscribeAndDiscard();
                });
            }

            private static void VerifyLazyRunAsync(Action<IRxResult> action)
            {
                var asyncTxc = new Mock<ITransaction>();
                asyncTxc.Setup(x => x.RunAsync(It.IsAny<Statement>()))
                    .ReturnsAsync(new ListBasedRecordCursor(new[] {"x"}, Enumerable.Empty<IRecord>,
                        Mock.Of<IResultSummary>));
                var txc = new InternalRxTransaction(asyncTxc.Object);
                var result = txc.Run("RETURN 1");

                asyncTxc.Verify(
                    x => x.RunAsync(It.IsAny<Statement>()), Times.Never);

                action(result);

                asyncTxc.Verify(
                    x => x.RunAsync(It.IsAny<Statement>()), Times.Once);
            }
        }

        public class Commit : AbstractRxTest
        {
            [Fact]
            public void ShouldInvokeTxcCommitAsync()
            {
                var asyncTxc = new Mock<ITransaction>();
                var rxTxc = new InternalRxTransaction(asyncTxc.Object);

                var commit = rxTxc.Commit<Unit>();

                asyncTxc.Verify(x => x.CommitAsync(), Times.Never);

                commit.SubscribeAndDiscard();

                asyncTxc.Verify(x => x.CommitAsync(), Times.Once);
            }
        }

        public class Rollback : AbstractRxTest
        {
            [Fact]
            public void ShouldInvokeTxcRollbackAsync()
            {
                var asyncTxc = new Mock<ITransaction>();
                var rxTxc = new InternalRxTransaction(asyncTxc.Object);

                var rollback = rxTxc.Rollback<Unit>();

                asyncTxc.Verify(x => x.RollbackAsync(), Times.Never);

                rollback.SubscribeAndDiscard();

                asyncTxc.Verify(x => x.RollbackAsync(), Times.Once);
            }
        }
    }
}