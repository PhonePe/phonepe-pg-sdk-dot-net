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

using NUnit.Framework;

namespace pg_sdk_dotnet.tests;

[TestFixture]
public class MetaInfoTests
{
    [Test]
    public void Build_WithAllUdfFields_ShouldSetAllProperties()
    {
        var metaInfo = MetaInfo.Builder()
            .SetUdf1("udf1")
            .SetUdf2("udf2")
            .SetUdf3("udf3")
            .SetUdf4("udf4")
            .SetUdf5("udf5")
            .SetUdf6("udf6")
            .SetUdf7("udf7")
            .SetUdf8("udf8")
            .SetUdf9("udf9")
            .SetUdf10("udf10")
            .SetUdf11("udf11")
            .SetUdf12("udf12")
            .SetUdf13("udf13")
            .SetUdf14("udf14")
            .SetUdf15("udf15")
            .Build();

        Assert.That(metaInfo.Udf1, Is.EqualTo("udf1"));
        Assert.That(metaInfo.Udf2, Is.EqualTo("udf2"));
        Assert.That(metaInfo.Udf3, Is.EqualTo("udf3"));
        Assert.That(metaInfo.Udf4, Is.EqualTo("udf4"));
        Assert.That(metaInfo.Udf5, Is.EqualTo("udf5"));
        Assert.That(metaInfo.Udf6, Is.EqualTo("udf6"));
        Assert.That(metaInfo.Udf7, Is.EqualTo("udf7"));
        Assert.That(metaInfo.Udf8, Is.EqualTo("udf8"));
        Assert.That(metaInfo.Udf9, Is.EqualTo("udf9"));
        Assert.That(metaInfo.Udf10, Is.EqualTo("udf10"));
        Assert.That(metaInfo.Udf11, Is.EqualTo("udf11"));
        Assert.That(metaInfo.Udf12, Is.EqualTo("udf12"));
        Assert.That(metaInfo.Udf13, Is.EqualTo("udf13"));
        Assert.That(metaInfo.Udf14, Is.EqualTo("udf14"));
        Assert.That(metaInfo.Udf15, Is.EqualTo("udf15"));
    }

    [Test]
    public void Build_WithNoFields_ShouldHaveAllNullUdfs()
    {
        var metaInfo = MetaInfo.Builder().Build();

        Assert.That(metaInfo.Udf1, Is.Null);
        Assert.That(metaInfo.Udf2, Is.Null);
        Assert.That(metaInfo.Udf3, Is.Null);
        Assert.That(metaInfo.Udf4, Is.Null);
        Assert.That(metaInfo.Udf5, Is.Null);
        Assert.That(metaInfo.Udf6, Is.Null);
        Assert.That(metaInfo.Udf7, Is.Null);
        Assert.That(metaInfo.Udf8, Is.Null);
        Assert.That(metaInfo.Udf9, Is.Null);
        Assert.That(metaInfo.Udf10, Is.Null);
        Assert.That(metaInfo.Udf11, Is.Null);
        Assert.That(metaInfo.Udf12, Is.Null);
        Assert.That(metaInfo.Udf13, Is.Null);
        Assert.That(metaInfo.Udf14, Is.Null);
        Assert.That(metaInfo.Udf15, Is.Null);
    }
}
