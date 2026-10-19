/*
* Copyright (c) 2025 Original Author(s), PhonePe India Pvt. Ltd.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System.Text.Json;
using NUnit.Framework;
using ResponsePaymentInstrumentType = pg_sdk_dotnet.Common.Models.Response.PaymentInstruments.PaymentInstrumentType;

namespace pg_sdk_dotnet.tests;

[NonParallelizable]
[TestFixture]
public class PaymentInstrumentsTests : BaseSetupWithOAuth
{
    
    [Test, Order(1)]
    public void CreditCardPaymentInstrumentV2_ShouldInitializeAndSetProperties()
    {
        var instrument = new CreditCardPaymentInstrumentV2
        {
            Type = ResponsePaymentInstrumentType.CREDIT_CARD,
            BankTransactionId = "BTID123",
            BankId = "BANK456",
            Brn = "BRN789",
            Arn = "ARN012"
        };

        Assert.That(instrument.Type, Is.EqualTo(ResponsePaymentInstrumentType.CREDIT_CARD));
        Assert.That(instrument.BankTransactionId, Is.EqualTo("BTID123"));
        Assert.That(instrument.BankId, Is.EqualTo("BANK456"));
        Assert.That(instrument.Brn, Is.EqualTo("BRN789"));
        Assert.That(instrument.Arn, Is.EqualTo("ARN012"));
    }

    [Test, Order(2)]
    public void DebitCardPaymentInstrumentV2_ShouldInitializeAndSetProperties()
    {
        var instrument = new DebitCardPaymentInstrumentV2
        {
            Type = ResponsePaymentInstrumentType.DEBIT_CARD,
            BankTransactionId = "BTID123",
            BankId = "BANK456",
            Brn = "BRN789",
            Arn = "ARN012"
        };

        Assert.That(instrument.Type, Is.EqualTo(ResponsePaymentInstrumentType.DEBIT_CARD));
        Assert.That(instrument.BankTransactionId, Is.EqualTo("BTID123"));
        Assert.That(instrument.BankId, Is.EqualTo("BANK456"));
        Assert.That(instrument.Brn, Is.EqualTo("BRN789"));
        Assert.That(instrument.Arn, Is.EqualTo("ARN012"));
    }

    [Test, Order(3)]
    public void EgvPaymentInstrumentV2_ShouldInitializeAndSetProperties(){
        var instrument = new EgvPaymentInstrumentV2
        {
            Type = ResponsePaymentInstrumentType.EGV,
            CardNumber = "1234567890123456",
            ProgramId = "PROGRAM123"
        };

        Assert.That(instrument.Type, Is.EqualTo(ResponsePaymentInstrumentType.EGV));
        Assert.That(instrument.CardNumber, Is.EqualTo("1234567890123456"));
        Assert.That(instrument.ProgramId, Is.EqualTo("PROGRAM123"));
    }

    [Test, Order(4)]
    public void WalletPaymentInstrumentV2_ShouldInitializeAndSetProperties()
    {
        var instrument = new WalletPaymentInstrumentV2
        {
            Type = ResponsePaymentInstrumentType.WALLET,
            WalletId = "WALLET123"
        };

        Assert.That(instrument.Type, Is.EqualTo(ResponsePaymentInstrumentType.WALLET));
        Assert.That(instrument.WalletId, Is.EqualTo("WALLET123"));
    }

    [Test, Order(5)]
    public void NetbankingPaymentInstrumentV2_ShouldInitializeAndSetProperties()
    {
        var instrument = new NetbankingPaymentInstrumentV2
        {
            Type = ResponsePaymentInstrumentType.NET_BANKING,
            BankTransactionId = "BTID123",
            BankId = "BANK456",
            Brn = "BRN789",
            Arn = "ARN012"
        };

        Assert.That(instrument.Type, Is.EqualTo(ResponsePaymentInstrumentType.NET_BANKING));
        Assert.That(instrument.BankTransactionId, Is.EqualTo("BTID123"));
        Assert.That(instrument.BankId, Is.EqualTo("BANK456"));
        Assert.That(instrument.Brn, Is.EqualTo("BRN789"));
        Assert.That(instrument.Arn, Is.EqualTo("ARN012"));
    }

    [Test, Order(6)]
    public void Deserialize_CreditCardPaymentInstrumentV2_WithCustomConverter()
    {
        var json = """
        {
            "type": "CREDIT_CARD",
            "bankTransactionId": "BTID123",
            "bankId": "BANK456",
            "brn": "BRN789",
            "arn": "ARN012"
        }
        """;

        var result = JsonSerializer.Deserialize<PaymentInstrumentV2>(json, JsonOptions.PaymentInstrumentDeserialization);

        Assert.That(result, Is.InstanceOf<CreditCardPaymentInstrumentV2>());
        var creditCard = (CreditCardPaymentInstrumentV2)result!;
        Assert.That(creditCard.BankTransactionId, Is.EqualTo("BTID123"));
        Assert.That(creditCard.BankId, Is.EqualTo("BANK456"));
        Assert.That(creditCard.Brn, Is.EqualTo("BRN789"));
        Assert.That(creditCard.Arn, Is.EqualTo("ARN012"));
    }

    [Test, Order(7)]
    public void Deserialize_UnknownType_ThrowsJsonException()
    {
        var json = """
        {
            "type": "UNKNOWN_TYPE"
        }
        """;

        Assert.Throws<JsonException>(() =>
        {
            JsonSerializer.Deserialize<PaymentInstrumentV2>(json, JsonOptions.PaymentInstrumentDeserialization);
        });
    }

    [Test, Order(8)]
    public void Deserialize_AccountInstrumentV2_WithProviderAccountType()
    {
        var json = """
        {
            "type": "ACCOUNT",
            "maskedAccountNumber": "XXXXXXXXXXXX1234",
            "ifsc": "IFSC0001234",
            "accountHolderName": "NAME",
            "accountType": "SAVINGS",
            "providerAccountType": "CREDITLINE"
        }
        """;

        var result = JsonSerializer.Deserialize<PaymentInstrumentV2>(json, JsonOptions.PaymentInstrumentDeserialization);

        Assert.That(result, Is.InstanceOf<AccountInstrumentV2>());
        var account = (AccountInstrumentV2)result!;
        Assert.That(account.Type, Is.EqualTo(ResponsePaymentInstrumentType.ACCOUNT));
        Assert.That(account.MaskedAccountNumber, Is.EqualTo("XXXXXXXXXXXX1234"));
        Assert.That(account.Ifsc, Is.EqualTo("IFSC0001234"));
        Assert.That(account.AccountHolderName, Is.EqualTo("NAME"));
        Assert.That(account.AccountType, Is.EqualTo("SAVINGS"));
        Assert.That(account.ProviderAccountType, Is.EqualTo("CREDITLINE"));
    }

    [Test, Order(9)]
    public void Deserialize_CreditLineInstrument_MapsToAccountInstrumentV2()
    {
        var json = """
        {
            "type": "CREDIT_LINE",
            "maskedAccountNumber": "XXXXXXXXXXXX1234",
            "ifsc": "IFSC0001234",
            "accountHolderName": "NAME",
            "accountType": "SAVINGS",
            "providerAccountType": "CREDITLINE"
        }
        """;

        var result = JsonSerializer.Deserialize<PaymentInstrumentV2>(json, JsonOptions.PaymentInstrumentDeserialization);

        Assert.That(result, Is.InstanceOf<AccountInstrumentV2>());
        var account = (AccountInstrumentV2)result!;
        Assert.That(account.Type, Is.EqualTo(ResponsePaymentInstrumentType.CREDIT_LINE));
        Assert.That(account.MaskedAccountNumber, Is.EqualTo("XXXXXXXXXXXX1234"));
        Assert.That(account.Ifsc, Is.EqualTo("IFSC0001234"));
        Assert.That(account.AccountHolderName, Is.EqualTo("NAME"));
        Assert.That(account.AccountType, Is.EqualTo("SAVINGS"));
        Assert.That(account.ProviderAccountType, Is.EqualTo("CREDITLINE"));
    }
}
