# BitcoinLibFSharp

This F# library provides the ability to generate bitcoin addresses using a randomly generated private key or your own private key. Your own private key can be supplied in raw binary form, hexadecimal format, or Wallet Input Format (WIF). The library provides information about the address, including: 
* Private Key in Hexadecimal
* Private Key in Wallet Input Format
* Public Key Compressed
* Public Key Full
* Public Key Hash160
* Pay To Public Key Hash (V1) Address in Hexadecimal
* Pay To Public Key Hash (V1) Address in Base58
* Pay To Script Hash Address in Hexadecimal
* Pay To Script Hash Address in Base58

## Usage

To generate a new address from a random private key, use the following:
```f#
    let newAddress = BitcoinAddress.GenerateNewRandomBitcoinAddressRecord true
    
    // Outputs: Private Key: 52cd0f5bcb679d4a134cd370b023f5c4c7a9462292b82525d5839a26c2a2f671 
    Console.WriteLine("Private Key: {0}", newAddress.PrivateKeyHex)
    
    // Outputs: Public Address: 1BXC75ckkMw59hEAj4UJuJX1t1WBrdoVLm
    Console.WriteLine("Public Address: {0}", newAddress.P2PKHAddress)
```

To generate an address from a Wallet Input Format key, use the following:
```f#
    let address = BitcoinAddress.GenerateBitcoinAddressRecordFromPrivateKeyWIF true "Kx45GeUBSMPReYQwgXiKhG9FzNXrnCeutJp4yjTd5kKxCitadm3C"
    
    // Outputs: Private Key: 18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725 
    Console.WriteLine("Private Key: {0}", address.PrivateKeyHex)
    
    // Outputs: Public Address: 1PMycacnJaSqwwJqjawXBErnLsZ7RkXUAs
    Console.WriteLine("Public Address: {0}", address.P2PKHAddress)
```

## Future Work
The things I plan on implementing next include:
1) Bech32 address support
2) Block parsing and block chain parsing/validation
3) Network Protocol support

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)
