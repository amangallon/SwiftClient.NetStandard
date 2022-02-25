﻿using System;
using System.Collections.Generic;

namespace SwiftClient.Cli
{
    public static class ListCommand
    {
        public static int Run(ListOptions options, Client client)
        {
            if (string.IsNullOrEmpty(options.Container))
            {
                var accountData = client.GetAccountAsync().Result;
                if (accountData.IsSuccess)
                {
                    if (accountData.Containers != null && accountData.Containers.Count > 0)
                    {
                        var list = new List<SwiftContainer>();
                        foreach (var c in accountData.Containers)
                        {
                            list.Add(new SwiftContainer
                            {
                                Bytes = c.Bytes,
                                Container = c.Container,
                                Objects = c.Objects
                            });
                        }

                        var table = list.ToStringTable(
                            u => u.Container,
                            u => u.Objects,
                            u => u.Size
                        );
                        Console.WriteLine(table);
                    }
                    else
                    {
                        Console.WriteLine("No containers found");
                    }
                }
                else
                {
                    Logger.LogError(accountData.Reason);
                }
            }
            else
            {
                var queryParams = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(options.Delimiter))
                {
                    queryParams.Add("delimiter", options.Delimiter);
                }

                if (!string.IsNullOrEmpty(options.Prefix))
                {
                    queryParams.Add("prefix", options.Prefix);
                }

                var containerData = client.GetContainerAsync(options.Container, null, queryParams).Result;
                if (containerData.IsSuccess)
                {
                    if (containerData.Objects != null && containerData.Objects.Count > 0)
                    {
                        var list = new List<SwiftObject>();
                        foreach (var c in containerData.Objects)
                        {
                            list.Add(new SwiftObject
                            {
                                Bytes = c.Bytes,
                                ContentType = c.ContentType,
                                Hash = c.Hash,
                                LastModified = c.LastModified,
                                Object = c.Object
                            });
                        }

                        var table = list.ToStringTable(
                            u => u.Object,
                            u => u.Size,
                            u => u.LastModified,
                            u => u.ContentType
                        );
                        Console.WriteLine(table);
                    }
                    else
                    {
                        Console.WriteLine($"Container is empty");
                    }
                }
                else
                {
                    Logger.LogError(containerData.Reason);
                }
            }
            return 0;
        }
    }
}
