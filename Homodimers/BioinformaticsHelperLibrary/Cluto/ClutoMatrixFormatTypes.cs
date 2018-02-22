using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.Cluto
{
    public enum ClutoMatrixFormatTypes
    {
        DenseMatrixComplete = 1,
        DenseMatrixTopHalf = 2,
        DenseMatrixBottomHalf = 3,
        SparseMatrixComplete = 4,
        SparseMatrixTopHalf = 5,
        SparseMatrixBottomHalf = 6,        
    }
}
