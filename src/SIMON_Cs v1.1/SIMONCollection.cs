
/*
 * 
 * SIMONObject 들을 관리하기 위한 클래스를 구조한다.
 * 
 * Default size는 일단 Constant에 정의한 Collection Default size로 1000을 잡아놓았다. 차후 조정할 예정이다.
 * 배열 기반으로 구현하고 Array Doubling 처리를 하며, key 값과 value를 매핑한 형태의 Dictionary 구조를 구현할 것이다.
 * 들어오는 Key 값은 유일하다. (SIMONObject의 ID가 들어오기 때문에 이 값은 유일함) 즉, 단순한 해싱함수(예를들어 %size 등)로 구현해도 무방할 것으로 보인다. -> 상의해볼것.
 * 
 *  
 */

using System;
using System.Collections;

namespace SIMONFramework
{
    /// <summary>
    /// Key값과 Value값을 순서쌍으로 갖는 SIMONObjectCollection Dictionary Abstraction Data Type을 제공합니다.
    /// </summary>
    public class SIMONCollection : IDictionary
    {
        /// <summary>
        /// Dictionary 내에 구현된 element를 저장하는 배열.
        /// </summary>
        public DictionaryEntry[] SIMONDictionary { get; private set; }
        /// <summary>
        /// 배열 사이즈.
        /// </summary>
        public int CollectionSize { get; private set; }
        /// <summary>
        /// Collection에 대한 Access 동기화 object.
        /// </summary>
        private object collectionSyncLocker; 

        #region IDictionary Interface 속성
        /// <summary>
        /// Collection의 크기가 정해져있는지를 검사합니다.
        /// </summary>
        public bool IsFixedSize { get; private set; }
        /// <summary>
        /// Collection이 읽기 전용인지를 검사합니다.
        /// </summary>
        public bool IsReadOnly { get; private set; }
        /// <summary>
        /// Collection 내의 모든 Key를 나열합니다.
        /// </summary>
        public ICollection Keys { get; private set; }
        /// <summary>
        /// Collection 내의 모든 Value를 나열합니다.
        /// </summary>
        public ICollection Values { get; private set; }
        /// <summary>
        /// Collection 내의 element 수를 저장합니다.
        /// </summary>
        public int Count { get; private set; }
        #endregion

        #region Synchronize 부분. -> 확인의 여지가 있을 것으로 보인다.
        public bool IsSynchronized { get { return false; } }                            //동기화 여부를 갖는 프로퍼티를 선언한다.
        public object SyncRoot { get { return collectionSyncLocker; } }                 //컬렉션에 대한 접근을 동기화할 때 사용하는 객체. Locker Object를 소유함으로써 멀티 쓰레드 환경에서 Mutual Exclusion을 구현한다.
        #endregion

        public SIMONCollection()
        {
            //Default 생성자.
            CollectionSize = SIMONConstants.DEFAULT_COLLECTION_SIZE;
            SIMONDictionary = new DictionaryEntry[CollectionSize];
            Count = 0;
            collectionSyncLocker = new object();
        }

        /// <summary>
        /// index 값에 위치하는 키값을 조회합니다.
        /// </summary>
        /// <param name="index">index 값입니다.</param>
        /// <returns>index에 해당하는 key값입니다.</returns>
        public object KeyOfIndex(int index)
        {
            object val = ValueOfIndex(index);
            for (int i = 0; i < Count; i++)
            {
                if (SIMONDictionary[i].Value.Equals(val))
                {
                    return SIMONDictionary[i].Key;
                }
            }
            return SIMONConstants.INVALID_COLLECTION_VALUE;
        }

        /// <summary>
        /// Key에 해당하는 Index 넘버를 반환하는 함수.
        /// </summary>
        /// <param name="key">key값입니다.</param>
        /// <returns>key에 해당하는 index값입니다.</returns>
        private int IndexOfKey(object key)
        {
            //key에 해당하는 Index 넘버를 반환.
            for (int i = 0; i < Count; i++)
                if (SIMONDictionary[i].Key.Equals(key))
                    return i;
            return SIMONConstants.INVALID_COLLECTION_INDEX;
        }

        /// <summary>
        /// ArrayDoubling을 통한 Dynamic Array 구조를 구현합니다.
        /// </summary>
        /// <returns>ArrayDoubling 연산의 수행 결과.</returns>
        private bool ArrayDoubling()
        {
            //DynamicArray 구조 구현.
            bool ret = true;
            try
            {
                DictionaryEntry[] backBuffer = new DictionaryEntry[Count];
                Array.Copy(SIMONDictionary, backBuffer, Count);
                SIMONDictionary = new DictionaryEntry[2 * Count];
                Array.Copy(backBuffer, SIMONDictionary, Count);
            }
            catch (AccessViolationException e)
            {
                ret = false;
                throw new SIMONFramework.AccessViolationException(SIMONConstants.EXP_ACCESS_VIOLATION_MSG);
            }
            catch (Exception e)
            {
                ret = false;
                throw e;
            }
            return ret;
        }

        /// <summary>
        /// Index만으로 Dictionary의 정보를 조회할 수 있는 기능을 구현합니다.
        /// </summary>
        /// <param name="index">index 값입니다.</param>
        /// <returns>index값으로 조회할 value입니다.</returns>
        public object ValueOfIndex(int index){
            if (index >= CollectionSize)
            {
                throw new SIMONFramework.IndexOutOfRangeException(SIMONConstants.EXP_IDX_OUT_OF_RANGE_MSG);
            }
            return SIMONDictionary[index].Value;
        }

        #region IDictionary Interface Override Method

        /// <summary>
        /// key와 value를 파라미터 값으로 전달함으로써 SIMONObjectCollection에 해당 요소를 추가합니다.
        /// </summary>
        /// <param name="key">입력할 key값입니다.</param>
        /// <param name="value">입력할 value값입니다.</param>
        public void Add(object key, object value)
        {
            //key값과 value를 갖는 새로운 요소를 컬렉션에 추가한다.
            if (key == null)
                throw new SIMONFramework.ArgumentNullException(SIMONConstants.EXP_ARG_NULL_MSG + key.ToString());
            if (value == null)
                throw new SIMONFramework.ArgumentNullException(SIMONConstants.EXP_ARG_NULL_MSG + value.ToString());
            if (Count.Equals(SIMONDictionary.Length))
            {
                //현재 들어있는 엘리먼트의 갯수가 길이와 같을경우(가득 차있는 경우), Array Doubling 작업 수행하고, value를 푸시해줘야함. 
                if (!ArrayDoubling())
                    throw new SIMONFramework.IndexOutOfRangeException(SIMONConstants.EXP_IDX_OUT_OF_RANGE_MSG);
                CollectionSize *= 2;                                                        //컬렉션의 사이즈를 2배로 만듬.
                SIMONDictionary[Count++] = new DictionaryEntry(key, value);
            }
            else
                SIMONDictionary[Count++] = new DictionaryEntry(key, value);
        }

        /// <summary>
        /// 본 SIMONObjectCollection의 모든 Element들을 비웁니다.
        /// </summary>
        public void Clear()
        {
            //컬렉션을 비운다. Value는 건드리지 않고, key값만 모두 null로 바꿔줘도 외부에서 직접 컬렉션에 접근이 불가능하고 카운트가 0으로 변하기 때문에 새로운 값이 들어오면 덮어쓰게 된다.
            //덮어씌여진 오브젝트는 가비지컬렉터에 의해 처리된다.
            for (int i = 0; i < CollectionSize; i++)
                SIMONDictionary[i].Key = null;
            CollectionSize = SIMONConstants.DEFAULT_COLLECTION_SIZE;                        //Array doubling이 일어나야할 이유가 없기 때문에 다시 크기를 Default 사이즈로 맞춰놓는다.
            Count = 0;                                                                      //사용 Count==0.
        }

        /// <summary>
        /// 파라미터로 전달받는 key값이 현재 SIMONObjectCollection에 존재하는지 여부를 반환합니다.
        /// </summary>
        /// <param name="key">확인할 key값입니다.</param>
        /// <returns>key가 존재하는지 여부를 반환합니다.</returns>
        public bool Contains(object key)
        {
            //특정 키가 데이터와 연관되어 있는지를 검사한다.
            if (key == null)
                throw new SIMONFramework.ArgumentNullException(SIMONConstants.EXP_ARG_NULL_MSG + key.ToString());
            for (int i = 0; i < Count; i++)
                if (SIMONDictionary[i].Key.Equals(key))
                    return true;
            return false;
        }

        System.Collections.IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary)this).GetEnumerator();
        }

        /// <summary>
        /// key와 value를 나열한다. Collection을 반복하는 열거자를 반환합니다.
        /// </summary>
        /// <returns>Collection 에 대한 반복 열거자입니다.</returns>
        public System.Collections.IDictionaryEnumerator GetEnumerator()
        {
            //키와 값을 나열한다. Collection을 반복하는 열거자를 반환, IDictionaryEnumerator 인터페이스를 상속한 SIMONObjectCollectionEnumerator를 반환한다.
            return new SIMONObjectCollectionEnumerator(this);
        }

        /// <summary>
        /// key값에 해당하는 Element를 SIMONObjectCollection에서 제거합니다.
        /// </summary>
        /// <param name="key">삭제할 element의 key값입니다.</param>
        public void Remove(object key)
        {
            //삭제할 값의 키를 전달하여 데이터를 컬렉션에서 삭제한다.
            if (key == null)
                throw new SIMONFramework.ArgumentNullException(SIMONConstants.EXP_ARG_NULL_MSG + key.ToString());
            int index = IndexOfKey(key);
            if (index != SIMONConstants.INVALID_COLLECTION_INDEX)
            {
                //해당 인덱스 뒤의 요소들 부터를 전부 한칸씩 땡겨서 저장시킴.
                for (int i = index; i < (Count - 1); i++)
                    SIMONDictionary[i] = SIMONDictionary[i + 1];
                Count--;                                                       //전체 카운트수 감소.
            }
            else
                throw new SIMONFramework.ArgumentNullException(SIMONConstants.EXP_ARG_NULL_MSG + key.ToString());              //키가 Dictionary에 없을 경우 예외 반환.
        }

        /// <summary>
        /// SIMONObjectCollection은 다른 Array로의 복사를 지원하지 않습니다.
        /// </summary>
        /// <param name="array">대상 Array입니다.</param>
        /// <param name="index">index 입니다.</param>
        public void CopyTo(Array array, int index)
        {
            //SIMONCollection은 다른 Array로의 복사를 (일단) 제공하지 않는/다.
            throw new SIMONFramework.NotImplementedException(SIMONConstants.EXP_NOT_IMPLEMENT_MSG);
        }

        /// <summary>
        /// key값을 통해서 Object value에 직접 접근하는 this 메서드를 정의합니다.
        /// </summary>
        /// <param name="key">Collection을 조회하는데 사용될 key값입니다.</param>
        /// <returns>Value값을 return합니다.</returns>
        public object this[object key]
        {
            get
            {
                int index = IndexOfKey(key);
                if (index != SIMONConstants.INVALID_COLLECTION_INDEX)
                    return SIMONDictionary[index];
                else
                    return SIMONConstants.INVALID_SIMON_OBJECT;
            }
            set
            {
                int index = IndexOfKey(key);
                if (index != SIMONConstants.INVALID_COLLECTION_INDEX)
                    SIMONDictionary[index].Value = value;               //value?
                else
                    Add(key, value);
            }
        }

        #endregion

        private class SIMONObjectCollectionEnumerator : IDictionaryEnumerator
        {
            //IDictionaryEnumerator 인터페이스를 상속받아서 SIMONObjectCollection에 대한 반복 열거 클래스를 구현합니다.
            private DictionaryEntry[] SIMONDictionary;
            private int dicIndex = -1;
            public Object Key
            {
                //유효한 값이면 값을 반환한다.
                get
                {
                    if (OperationCheck().Equals(true))
                        return SIMONDictionary[dicIndex].Key;
                    else
                        return SIMONConstants.INVALID_COLLECTION_OPERATION;
                }
            }
            public Object Value
            {
                //유효한 값이면 값을 반환한다.
                get
                {
                    if (OperationCheck().Equals(true))
                        return SIMONDictionary[dicIndex].Value;
                    else
                        return SIMONConstants.INVALID_COLLECTION_OPERATION;
                }
            }
            public Object Current
            {
                //현재 엔트리의 값을 반환한다.
                get
                {
                    if (OperationCheck().Equals(true))
                        return SIMONDictionary[dicIndex];
                    else
                        return SIMONConstants.INVALID_COLLECTION_OPERATION;
                }
            }
            public DictionaryEntry Entry
            {
                //현재 엔트리를 조회한다.
                get
                {
                    return (DictionaryEntry)Current;
                }
            }

            public SIMONObjectCollectionEnumerator(SIMONCollection simonCollection)
            {
                SIMONDictionary = new DictionaryEntry[simonCollection.Count];
                Array.Copy(simonCollection.SIMONDictionary, 0, SIMONDictionary, 0, simonCollection.Count);
            }
            private bool OperationCheck()
            {
                //정상적인 수행인지를 판단해서 리턴한다.
                bool operChk = true;
                try
                {
                    if (dicIndex < 0 || dicIndex >= SIMONDictionary.Length)
                        throw new InvalidOperationException("Enumerator's index is invalid.");
                }
                catch (InvalidOperationException e)
                {
                    operChk = false;
                    Console.WriteLine(e.Message);
                }
                return operChk;
            }
            public Boolean MoveNext()
            {
                //현재 Dictionary를 가리키고 있는 인덱스 포인터를 이동시킨다.
                if (dicIndex < SIMONDictionary.Length - 1)
                {
                    dicIndex++;
                    return true;
                }
                return false;
            }
            public void Reset()
            {
                //Dictionary 열거자의 포인터를 초기화한다.
                dicIndex = -1;
            }
        }
    }
}