// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Http.Features.Internal
{
    public class TlsConnectionFeature : ITlsConnectionFeature
    {
        public TlsConnectionFeature()
        {
        }

        public X509Certificate2 ClientCertificate { get; set; }

        public Task<X509Certificate2> GetClientCertificateAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(ClientCertificate);
        }
    }
}