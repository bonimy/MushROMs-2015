using System;

namespace MushROMs
{
    public delegate int ValueTransformer(int value, object args);

    internal class AddressIndexConverter
    {
        private int index;
        private int address;

        private object indexArgs;
        private object addressArgs;

        private ValueTransformer AddressToIndex;
        private ValueTransformer IndexToAddress;

        public int Index
        {
            get { return this.index; }
            set
            {
                this.index = value;
                this.address = IndexToAddress(value, this.IndexToAddressArgs);
            }
        }

        public int Address
        {
            get { return this.address; }
            set
            {
                this.address = value;
                this.index = IndexToAddress(value, this.AddressToIndexArgs);
            }
        }

        public object IndexToAddressArgs
        {
            get { return this.indexArgs; }
            set { this.indexArgs = value; }
        }

        public object AddressToIndexArgs
        {
            get { return this.addressArgs; }
            set { this.addressArgs = value; }
        }

        public AddressIndexConverter(ValueTransformer indexToAddress, ValueTransformer addressToIndex)
        {
            if (indexToAddress == null)
                throw new ArgumentNullException("indexToAddress");
            if (addressToIndex == null)
                throw new ArgumentNullException("addressToIndex");

            this.indexArgs = null;
            this.addressArgs = null;

            this.index = 0;
            this.address = 0;

            this.IndexToAddress = indexToAddress;
            this.AddressToIndex = addressToIndex;
        }
    }
}