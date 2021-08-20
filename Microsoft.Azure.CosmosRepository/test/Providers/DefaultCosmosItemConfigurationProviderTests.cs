// Copyright (c) IEvangelist. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Cosmos;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Options;
using Microsoft.Azure.CosmosRepository.Providers;
using Moq;
using Xunit;

namespace Microsoft.Azure.CosmosRepositoryTests.Providers
{
    public class DefaultCosmosItemConfigurationProviderTests
    {
        private readonly Mock<ICosmosContainerNameProvider> _containerNameProvider = new();
        private readonly Mock<ICosmosPartitionKeyPathProvider> _partitionKeyPathProvider = new();
        private readonly Mock<ICosmosUniqueKeyPolicyProvider> _uniqueKeyPolicyProvider = new();


        [Fact]
        public void GetOptionsAlwaysGetOptionsForItem()
        {
            ICosmosItemConfigurationProvider provider = new DefaultCosmosItemConfigurationProvider(
                _containerNameProvider.Object,
                _partitionKeyPathProvider.Object,
                _uniqueKeyPolicyProvider.Object
            );

            UniqueKeyPolicy uniqueKeyPolicy = new();

            _containerNameProvider.Setup(o => o.GetContainerName<Item1>()).Returns("a");
            _partitionKeyPathProvider.Setup(o => o.GetPartitionKeyPath<Item1>()).Returns("/id");
            _uniqueKeyPolicyProvider.Setup(o => o.GetUniqueKeyPolicy<Item1>()).Returns(uniqueKeyPolicy);

            ItemOptions options = provider.GetOptions<Item1>();

            Assert.Equal("a", options.ContainerName);
            Assert.Equal("/id", options.PartitionKeyPath);
            Assert.Equal(uniqueKeyPolicy, options.UniqueKeyPolicy);
        }

        class Item1 : Item
        {

        }
    }
}