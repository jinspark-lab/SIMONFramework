
/*
 * 
 * SIMON Framework에서 사용되는 상수들을 관리하기 위한 클래스를 구조한다.
 * 
 * class SIMONConstants 안에 상수들을 정의하고 namespace안에서 가져다 씁니다.
 * 
 *  
 */

namespace SIMONFramework
{
    /// <summary>
    /// SIMONFramework에서 사용하는 상수값들을 정의해 놓은 클래스입니다.
    /// </summary>
    static class SIMONConstants
    {
        #region SIMONFramework 구조 관련 상수
        public const string DEFAULT_FUNCTION_CLSNAME = "SIMONUserFunction";
        #endregion

        public const double ERROR_WEIGHT = -9999.9999;
        public const double NULL_WEIGHT = -9999.9998;
        public const string UNKNOWN_NAME = "UNKNOWN";
        public const int DEFAULT_COLLECTION_SIZE = 1000;
        public const int INVALID_COLLECTION_INDEX = -1;
        public const int INVALID_COLLECTION_VALUE = -2;
        public const int INVALID_COLLECTION_OPERATION = -9999;
        public const object INVALID_SIMON_OBJECT = null;


        #region 학습 및 시뮬레이팅 관련 상수

        public const int DEFAULT_LEARNING_COUNT = 0;
        public const int DEFAULT_NOLEARN_POINT = -1;
        public const int DEFAULT_LEARNING_POINT = 1000;
        public const int DEFAULT_LEARNING_NULL = 0;
        public const int DEFAULT_NOLEARN_PARAM = -1;
        public const int DEFAULT_LEARN_PARAM = 0;

        #endregion

        #region 알고리즘적으로 사용되는 최대 및 최소 비교값

        public const int MINIMUM_CMP_VALUE = -987654321;
        public const int MAXIMUM_CMP_VALUE = 987654321;
        public const int FITNESS_CMP_ZERO = 0;
        public const int FITNESS_CMP_ONE = 1;
        public const double FITNESS_MAX_VALUE = 9999.9999;
        public const double FITNESS_MIN_VALUE = -9999.9999;

        #endregion

        #region SIMON 알고리즘에서 수치상으로 사용하는 Default 상수

        public const double GENE_MUTATION_CHANCE = 10;
        public const double GENE_MUTATION_PERCENT = 100;
        public const double GENE_MUTATION_PROPORTION = 20;
        public const int GENE_DOMINION_RATING = 3;
        public const int GENE_RECESSIVE_RATING = 1;
        public const int GENE_SUM_RATING = 4;
        public const int GENE_REAL_DOMINION_NUM = 3;
        public const int GENE_REAL_RECESSIVE_NUM = 1;
        public const int GENE_REAL_SELECT_NUM = 4;
        public const int GENE_POOL_MAXSMALL = 100;
        public const int GENE_POOL_MAXSIZE = 1000;

        #endregion


        #region Framework 관련 Directory workpath 계층 구조를 정의하는 상수

        public const string API_ROOT_PATH = @"\SIMON";
        public const string API_DEFINITION_PATH = API_ROOT_PATH + @"\Definitions";
        public const string API_HISTORY_PATH = API_ROOT_PATH + @"\Historys";
        //      public const string API_LOG_PATH = @"\Logs";

        #endregion

    }


}