﻿using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.CacheStore.Configuration
{
    /// <summary>
    /// 配置项
    /// </summary>
    public class CacheStoreOptions : IOptions<CacheStoreOptions>
    {
        /// <summary>
        /// Redis配置
        /// </summary>
        public RedisCacheStoreOptions Redis { get; set; }

        CacheStoreOptions IOptions<CacheStoreOptions>.Value
        {
            get { return this; }
        }

        public CacheStoreOptions()
        {
            Redis = new RedisCacheStoreOptions
            {
                Host = "127.0.0.1",
                Port = 6379,
                InstanceName = "0"
            };
        }
    }
}
