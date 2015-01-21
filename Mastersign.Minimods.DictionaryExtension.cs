#region Minimod
// <MiniMod>
//   <Name>Dictionary Extensions</Name>
//   <Author>Tobias Kiertscher &lt;dev@mastersign.de&gt;</Author>
//   <LastChanged>2015-01-21</LastChanged>
//   <Version>1.1.0</Version>
//   <Url>https://gist.github.com/mastersign/b9065d2cdcdbd13b78db/raw/Mastersign.Minimod.DictionaryExtension.cs</Url>
//   <Description>
//     This minimod contains extension methods for the IDictionary<TKey, TValue> interface.
//   </Description>
// </MiniMod>
#endregion

using System;
using System.Collections.Generic;

namespace Mastersign.Minimods
{
    /// <summary>
    /// This static class contains extension methods for <see cref="IDictionary{TKey,TValue}"/>.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        ///     Returns a value from the dictionary,
        ///     with a default value as fallback in case the given key does not exist.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary to search in.</param>
        /// <param name="key">The key to look-up.</param>
        /// <param name="defaultValue">
        ///     The optional default value, which is returned in case the <paramref name="key"/> is not found.
        ///     If no value is given for this parameter, <c>null</c> is used for reference types and 
        ///     a zero-initialized value is used for value types.
        /// </param>
        /// <returns>
        ///     The value looked-up from the <paramref name="dictionary"/> 
        ///     or <paramref name="defaultValue"/> in case the <paramref name="key"/> was not found.
        /// </returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key, TValue defaultValue = default(TValue))
        {
            TValue result;
            return dictionary.TryGetValue(key, out result)
                ? result
                : defaultValue;
        }

        /// <summary>
        ///     Returns a value from the dictionary,
        ///     with a function, generating a default value as fallback, in case the given key does not exist.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary to search in.</param>
        /// <param name="key">The key to look-up.</param>
        /// <param name="defaultGenerator">
        ///     A call-back funtion which is called in case the key was not found.
        ///     The return value of the function call is used as the fallback value.
        /// </param>
        /// <returns>
        ///     The value looked-up from the <paramref name="dictionary"/> 
        ///     or the return value of <paramref name="defaultGenerator"/> 
        ///     in case the <paramref name="key"/> was not found.
        /// </returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key, Func<TValue> defaultGenerator)
        {
            TValue result;
            return dictionary.TryGetValue(key, out result)
                ? result
                : defaultGenerator();
        }
    }
}