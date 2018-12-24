﻿using RedisUnitTest.Mock;
using Sino.Extensions.Redis;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace RedisUnitTest
{
    public class SetTests
    {
        [Fact]
        public void TestSAdd()
        {
            using (var mock = new FakeRedisSocket(":3\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.Equal(3, redis.SAdd("test", "test1"));
                Assert.Equal("*3\r\n$4\r\nSADD\r\n$4\r\ntest\r\n$5\r\ntest1\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void TestSCard()
        {
            using (var mock = new FakeRedisSocket(":3\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.Equal(3, redis.SCard("test"));
                Assert.Equal("*2\r\n$5\r\nSCARD\r\n$4\r\ntest\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void TestSDiff()
        {
            using (var mock = new FakeRedisSocket("*2\r\n$5\r\ntest1\r\n$5\r\ntest2\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                var response = redis.SDiff("test", "another");
                Assert.Equal(2, response.Length);
                Assert.Equal("test1", response[0]);
                Assert.Equal("test2", response[1]);
                Assert.Equal("*3\r\n$5\r\nSDIFF\r\n$4\r\ntest\r\n$7\r\nanother\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void TestSDiffStore()
        {
            using (var mock = new FakeRedisSocket(":3\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.Equal(3, redis.SDiffStore("destination", "key1", "key2"));
                Assert.Equal("*4\r\n$10\r\nSDIFFSTORE\r\n$11\r\ndestination\r\n$4\r\nkey1\r\n$4\r\nkey2\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void TestInter()
        {
            using (var mock = new FakeRedisSocket("*2\r\n$5\r\ntest1\r\n$5\r\ntest2\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                var response = redis.SInter("test", "another");
                Assert.Equal(2, response.Length);
                Assert.Equal("test1", response[0]);
                Assert.Equal("test2", response[1]);
                Assert.Equal("*3\r\n$6\r\nSINTER\r\n$4\r\ntest\r\n$7\r\nanother\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void TestSInterStore()
        {
            using (var mock = new FakeRedisSocket(":3\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.Equal(3, redis.SInterStore("destination", "key1", "key2"));
                Assert.Equal("*4\r\n$11\r\nSINTERSTORE\r\n$11\r\ndestination\r\n$4\r\nkey1\r\n$4\r\nkey2\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void TestSIsMember()
        {
            using (var mock = new FakeRedisSocket(":1\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.True(redis.SIsMember("test", "test1"));
                Assert.Equal("*3\r\n$9\r\nSISMEMBER\r\n$4\r\ntest\r\n$5\r\ntest1\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void TestSMembers()
        {
            using (var mock = new FakeRedisSocket("*2\r\n$5\r\ntest1\r\n$5\r\ntest2\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                var response = redis.SMembers("test");
                Assert.Equal(2, response.Length);
                Assert.Equal("test1", response[0]);
                Assert.Equal("test2", response[1]);
                Assert.Equal("*2\r\n$8\r\nSMEMBERS\r\n$4\r\ntest\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void TestSMove()
        {
            using (var mock = new FakeRedisSocket(":1\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.True(redis.SMove("test", "destination", "test1"));
                Assert.Equal("*4\r\n$5\r\nSMOVE\r\n$4\r\ntest\r\n$11\r\ndestination\r\n$5\r\ntest1\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void TestSPop()
        {
            using (var mock = new FakeRedisSocket("$5\r\ntest1\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.Equal("test1", redis.SPop("test"));
                Assert.Equal("*2\r\n$4\r\nSPOP\r\n$4\r\ntest\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void TestSRandMember()
        {
            using (var mock = new FakeRedisSocket("$5\r\ntest1\r\n", "*2\r\n$5\r\ntest1\r\n$5\r\ntest2\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.Equal("test1", redis.SRandMember("test"));
                Assert.Equal("*2\r\n$11\r\nSRANDMEMBER\r\n$4\r\ntest\r\n", mock.GetMessage());

                var response = redis.SRandMember("test", 2);
                Assert.Equal(2, response.Length);
                Assert.Equal("test1", response[0]);
                Assert.Equal("test2", response[1]);
                Assert.Equal("*3\r\n$11\r\nSRANDMEMBER\r\n$4\r\ntest\r\n$1\r\n2\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void TestSRem()
        {
            using (var mock = new FakeRedisSocket(":2\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.Equal(2, redis.SRem("test", "test1", "test2"));
                Assert.Equal("*4\r\n$4\r\nSREM\r\n$4\r\ntest\r\n$5\r\ntest1\r\n$5\r\ntest2\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void TestSScan()
        {
            string reply = "*2\r\n$2\r\n23\r\n*2\r\n$5\r\ntest1\r\n$5\r\ntest2\r\n";
            using (var mock = new FakeRedisSocket(reply, reply, reply, reply))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                var response1 = redis.SScan("test", 0);
                Assert.Equal(23, response1.Cursor);
                Assert.Equal(2, response1.Items.Length);
                Assert.Equal("test1", response1.Items[0]);
                Assert.Equal("test2", response1.Items[1]);
                Assert.Equal("*3\r\n$5\r\nSSCAN\r\n$4\r\ntest\r\n$1\r\n0\r\n", mock.GetMessage());

                var response2 = redis.SScan("test", 0, pattern: "*");
                Assert.Equal("*5\r\n$5\r\nSSCAN\r\n$4\r\ntest\r\n$1\r\n0\r\n$5\r\nMATCH\r\n$1\r\n*\r\n", mock.GetMessage());

                var response3 = redis.SScan("test", 0, count: 5);
                Assert.Equal("*5\r\n$5\r\nSSCAN\r\n$4\r\ntest\r\n$1\r\n0\r\n$5\r\nCOUNT\r\n$1\r\n5\r\n", mock.GetMessage());

                var response4 = redis.SScan("test", 0, "*", 5);
                Assert.Equal("*7\r\n$5\r\nSSCAN\r\n$4\r\ntest\r\n$1\r\n0\r\n$5\r\nMATCH\r\n$1\r\n*\r\n$5\r\nCOUNT\r\n$1\r\n5\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void TestSUnion()
        {
            using (var mock = new FakeRedisSocket("*2\r\n$5\r\ntest1\r\n$5\r\ntest2\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                var response = redis.SUnion("test", "another");
                Assert.Equal(2, response.Length);
                Assert.Equal("test1", response[0]);
                Assert.Equal("test2", response[1]);
                Assert.Equal("*3\r\n$6\r\nSUNION\r\n$4\r\ntest\r\n$7\r\nanother\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void TestSUnionStore()
        {
            using (var mock = new FakeRedisSocket(":3\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.Equal(3, redis.SUnionStore("destination", "key1", "key2"));
                Assert.Equal("*4\r\n$11\r\nSUNIONSTORE\r\n$11\r\ndestination\r\n$4\r\nkey1\r\n$4\r\nkey2\r\n", mock.GetMessage());
            }
        }
    }
}