import { enc, MD5, TripleDES, mode, pad } from 'crypto-js';

export class EncryptionUtil {
  static passphrase = 'ABCD'; // Replace with your actual passphrase

  static encryptData(Message: string): string {
    const UTF8 = enc.Utf8;
    const HashProvider = MD5;
    const TDESKey = HashProvider(UTF8.parse(this.passphrase));
    const TripleDes = TripleDES.encrypt(Message, TDESKey, {
      mode: mode.ECB,
      padding: pad.Pkcs7,
    });

    return TripleDes.toString();
  }
}
