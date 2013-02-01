#region MiniMod
// <MiniMod>
//   <Name>Matrix Proxy</Name>
//   <Author>Tobias Kiertscher</Author>
//   <LastChanged>2012-09-07</LastChanged>
//   <Version>1.0.0.0</Version>
//   <Url>https://raw.github.com/mastersign/minimods/master/de.mastersign.minimods.matrixproxy.cs</Url>
//   <Description>
//     This minimod allows the unified acces to different representations of 2D-matrices.
//     It allows the access to 2D-array, jagged array, and linear array.
//     Further, it supports row-column-order and column-row-order and
//     reversing of index directions.
//   </Description>
// </MiniMod>
#endregion

using System;
using System.Collections;
using System.Collections.Generic;

namespace de.mastersign.minimods.matrixproxy
{
    public interface IMatrixProxy<T> : IEnumerable<T>
    {
        object RawData { get; }

        int Columns { get; }

        int Rows { get; }

        T this[int row, int column] { get; set; }

        void Apply(Func<T, T> f);

        //void Apply(Func<int, int, T, T> f);

        void Fill(T value);

        void Fill(Func<T> f);

        //void Fill(Func<int, int, T> f);

        void ForEach(Action<T> action);

        //void ForEach(Action<int, int, T> action);
    }

    public enum MatrixOrder
    {
        RowColumn,
        ColumnRow,
    }

    public static class MatrixProxy
    {
        public static IMatrixProxy<T> Create<T>(T[,] data,
            MatrixOrder matrixOrder = MatrixOrder.RowColumn,
            bool reverseRowIndex = false,
            bool reverseColumnIndex = false)
        {
            return matrixOrder == MatrixOrder.RowColumn
                ? (!reverseRowIndex
                        ? (!reverseColumnIndex
                                ? (IMatrixProxy<T>)new MultiDimRowColumn<T>(data)
                                : new MultiDimRowColumnReverseColumns<T>(data))
                        : (!reverseColumnIndex
                                ? (IMatrixProxy<T>)new MultiDimRowColumnReverseRows<T>(data)
                                : new MultiDimRowColumnReverseRowsAndColumns<T>(data)))
                : (!reverseRowIndex
                        ? (!reverseColumnIndex
                                ? (IMatrixProxy<T>)new MultiDimColumnRow<T>(data)
                                : new MultiDimColumnRowReverseColumns<T>(data))
                        : (!reverseColumnIndex
                                ? (IMatrixProxy<T>)new MultiDimColumnRowReverseRows<T>(data)
                                : new MultiDimColumnRowReverseRowsAndColumns<T>(data)));
        }

        public static IMatrixProxy<T> Create<T>(T[][] data,
            MatrixOrder matrixOrder = MatrixOrder.RowColumn,
            bool reverseRowIndex = false,
            bool reverseColumnIndex = false)
        {
            return matrixOrder == MatrixOrder.RowColumn
                ? (!reverseRowIndex
                        ? (!reverseColumnIndex
                                ? (IMatrixProxy<T>)new JaggedArrayRowColumn<T>(data)
                                : new JaggedArrayRowColumnReverseColumns<T>(data))
                        : (!reverseColumnIndex
                                ? (IMatrixProxy<T>)new JaggedArrayRowColumnReverseRows<T>(data)
                                : new JaggedArrayRowColumnReverseRowsAndColumns<T>(data)))
                : (!reverseRowIndex
                        ? (!reverseColumnIndex
                                ? (IMatrixProxy<T>)new JaggedArrayColumnRow<T>(data)
                                : new JaggedArrayColumnRowReverseColumns<T>(data))
                        : (!reverseColumnIndex
                                ? (IMatrixProxy<T>)new JaggedArrayColumnRowReverseRows<T>(data)
                                : new JaggedArrayColumnRowReverseRowsAndColumns<T>(data)));
        }

        public static IMatrixProxy<T> Create<T>(T[] data,
            int offset, int rows, int columns,
            MatrixOrder matrixOrder = MatrixOrder.RowColumn,
            bool reverseRowIndex = false,
            bool reverseColumnIndex = false)
        {
            return matrixOrder == MatrixOrder.RowColumn
                ? (!reverseRowIndex
                        ? (!reverseColumnIndex
                                ? (IMatrixProxy<T>)new LinearArrayRowColumn<T>(data, offset, rows, columns)
                                : new LinearArrayRowColumnReverseColumns<T>(data, offset, rows, columns))
                        : (!reverseColumnIndex
                                ? (IMatrixProxy<T>)new LinearArrayRowColumnReverseRows<T>(data, offset, rows, columns)
                                : new LinearArrayRowColumnReverseRowsAndColumns<T>(data, offset, rows, columns)))
                : (!reverseRowIndex
                        ? (!reverseColumnIndex
                                ? (IMatrixProxy<T>)new LinearArrayColumnRow<T>(data, offset, rows, columns)
                                : new LinearArrayColumnRowReverseColumns<T>(data, offset, rows, columns))
                        : (!reverseColumnIndex
                                ? (IMatrixProxy<T>)new LinearArrayColumnRowReverseRows<T>(data, offset, rows, columns)
                                : new LinearArrayColumnRowReverseRowsAndColumns<T>(data, offset, rows, columns)));
        }

        private abstract class BasicMatrixProxy
        {
            public int Columns { get; protected set; }

            public int Rows { get; protected set; }
        }

        #region MultiDim

        private abstract class AbstractMultiDimProxy<T> : BasicMatrixProxy, IEnumerable<T>
        {
            protected readonly T[,] Data;

            public object RawData { get { return Data; } }

            protected AbstractMultiDimProxy(T[,] data)
            {
                Data = data;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<T> GetEnumerator()
            {
                var l1 = Data.GetLength(0);
                var l2 = Data.GetLength(1);
                for (var i = 0; i < l1; i++)
                {
                    for (var j = 0; j < l2; j++)
                    {
                        yield return Data[i, j];
                    }
                }
            }

            public void Apply(Func<T, T> f)
            {
                var l1 = Data.GetLength(0);
                var l2 = Data.GetLength(1);
                for (var i = 0; i < l1; i++)
                {
                    for (var j = 0; j < l2; j++)
                    {
                        Data[i, j] = f(Data[i, j]);
                    }
                }
            }

            public void Fill(T value)
            {
                var l1 = Data.GetLength(0);
                var l2 = Data.GetLength(1);
                for (var i = 0; i < l1; i++)
                {
                    for (var j = 0; j < l2; j++)
                    {
                        Data[i, j] = value;
                    }
                }
            }

            public void Fill(Func<T> f)
            {
                var l1 = Data.GetLength(0);
                var l2 = Data.GetLength(1);
                for (var i = 0; i < l1; i++)
                {
                    for (var j = 0; j < l2; j++)
                    {
                        Data[i, j] = f();
                    }
                }
            }

            public void ForEach(Action<T> action)
            {
                var l1 = Data.GetLength(0);
                var l2 = Data.GetLength(1);
                for (var i = 0; i < l1; i++)
                {
                    for (var j = 0; j < l2; j++)
                    {
                        action(Data[i, j]);
                    }
                }
            }
        }

        private class MultiDimRowColumn<T> : AbstractMultiDimProxy<T>, IMatrixProxy<T>
        {
            public MultiDimRowColumn(T[,] data)
                : base(data)
            {
                Rows = data.GetLength(0);
                Columns = data.GetLength(1);
            }

            public T this[int row, int column]
            {
                get { return Data[row, column]; }
                set { Data[row, column] = value; }
            }
        }

        private class MultiDimRowColumnReverseRows<T> : AbstractMultiDimProxy<T>, IMatrixProxy<T>
        {
            public MultiDimRowColumnReverseRows(T[,] data)
                : base(data)
            {
                Rows = data.GetLength(0);
                Columns = data.GetLength(1);
            }

            public T this[int row, int column]
            {
                get { return Data[Rows - 1 - row, column]; }
                set { Data[Rows - 1 - row, column] = value; }
            }
        }

        private class MultiDimRowColumnReverseColumns<T> : AbstractMultiDimProxy<T>, IMatrixProxy<T>
        {
            public MultiDimRowColumnReverseColumns(T[,] data)
                : base(data)
            {
                Rows = data.GetLength(0);
                Columns = data.GetLength(1);
            }

            public T this[int row, int column]
            {
                get { return Data[row, Columns - 1 - column]; }
                set { Data[row, Columns - 1 - column] = value; }
            }
        }

        private class MultiDimRowColumnReverseRowsAndColumns<T> : AbstractMultiDimProxy<T>, IMatrixProxy<T>
        {
            public MultiDimRowColumnReverseRowsAndColumns(T[,] data)
                : base(data)
            {
                Rows = data.GetLength(0);
                Columns = data.GetLength(1);
            }

            public T this[int row, int column]
            {
                get { return Data[Rows - 1 - row, Columns - 1 - column]; }
                set { Data[Rows - 1 - row, Columns - 1 - column] = value; }
            }
        }

        private class MultiDimColumnRow<T> : AbstractMultiDimProxy<T>, IMatrixProxy<T>
        {
            public MultiDimColumnRow(T[,] data)
                : base(data)
            {
                Rows = data.GetLength(1);
                Columns = data.GetLength(0);
            }

            public T this[int row, int column]
            {
                get { return Data[column, row]; }
                set { Data[column, row] = value; }
            }
        }

        private class MultiDimColumnRowReverseRows<T> : AbstractMultiDimProxy<T>, IMatrixProxy<T>
        {
            public MultiDimColumnRowReverseRows(T[,] data)
                : base(data)
            {
                Rows = data.GetLength(1);
                Columns = data.GetLength(0);
            }

            public T this[int row, int column]
            {
                get { return Data[column, Rows - 1 - row]; }
                set { Data[column, Rows - 1 - row] = value; }
            }
        }

        private class MultiDimColumnRowReverseColumns<T> : AbstractMultiDimProxy<T>, IMatrixProxy<T>
        {
            public MultiDimColumnRowReverseColumns(T[,] data)
                : base(data)
            {
                Rows = data.GetLength(1);
                Columns = data.GetLength(0);
            }

            public T this[int row, int column]
            {
                get { return Data[Columns - 1 - column, row]; }
                set { Data[Columns - 1 - column, row] = value; }
            }
        }

        private class MultiDimColumnRowReverseRowsAndColumns<T> : AbstractMultiDimProxy<T>, IMatrixProxy<T>
        {
            public MultiDimColumnRowReverseRowsAndColumns(T[,] data)
                : base(data)
            {
                Rows = data.GetLength(1);
                Columns = data.GetLength(0);
            }

            public T this[int row, int column]
            {
                get { return Data[Columns - 1 - column, Rows - 1 - row]; }
                set { Data[Columns - 1 - column, Rows - 1 - row] = value; }
            }
        }

        #endregion

        #region Jagged Array

        private class AbstractJaggedArrayProxy<T> : BasicMatrixProxy, IEnumerable<T>
        {
            protected readonly T[][] Data;

            public object RawData { get { return Data; } }

            protected AbstractJaggedArrayProxy(T[][] data)
            {
                Data = data;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<T> GetEnumerator()
            {
                var l1 = Data.GetLength(0);
                var l2 = Data.GetLength(1);
                for (var i = 0; i < l1; i++)
                {
                    var v = Data[i];
                    for (var j = 0; j < l2; j++)
                    {
                        yield return v[j];
                    }
                }
            }

            public void Apply(Func<T, T> f)
            {
                var l1 = Data.GetLength(0);
                var l2 = Data.GetLength(1);
                for (var i = 0; i < l1; i++)
                {
                    var v = Data[i];
                    for (var j = 0; j < l2; j++)
                    {
                        v[j] = f(v[j]);
                    }
                }
            }

            public void Fill(T value)
            {
                var l1 = Data.GetLength(0);
                var l2 = Data.GetLength(1);
                for (var i = 0; i < l1; i++)
                {
                    var v = Data[i];
                    for (var j = 0; j < l2; j++)
                    {
                        v[j] = value;
                    }
                }
            }

            public void Fill(Func<T> f)
            {
                var l1 = Data.GetLength(0);
                var l2 = Data.GetLength(1);
                for (var i = 0; i < l1; i++)
                {
                    var v = Data[i];
                    for (var j = 0; j < l2; j++)
                    {
                        v[j] = f();
                    }
                }
            }

            public void ForEach(Action<T> action)
            {
                var l1 = Data.GetLength(0);
                var l2 = Data.GetLength(1);
                for (var i = 0; i < l1; i++)
                {
                    var v = Data[i];
                    for (var j = 0; j < l2; j++)
                    {
                        action(v[j]);
                    }
                }
            }
        }

        private class JaggedArrayRowColumn<T> : AbstractJaggedArrayProxy<T>, IMatrixProxy<T>
        {
            public JaggedArrayRowColumn(T[][] data)
                : base(data)
            {
                Rows = data.GetLength(0);
                Columns = data.GetLength(1);
            }

            public T this[int row, int column]
            {
                get { return Data[row][column]; }
                set { Data[row][column] = value; }
            }
        }

        private class JaggedArrayRowColumnReverseRows<T> : AbstractJaggedArrayProxy<T>, IMatrixProxy<T>
        {
            public JaggedArrayRowColumnReverseRows(T[][] data)
                : base(data)
            {
                Rows = data.GetLength(0);
                Columns = data.GetLength(1);
            }

            public T this[int row, int column]
            {
                get { return Data[Rows - 1 - row][column]; }
                set { Data[Rows - 1 - row][column] = value; }
            }
        }

        private class JaggedArrayRowColumnReverseColumns<T> : AbstractJaggedArrayProxy<T>, IMatrixProxy<T>
        {
            public JaggedArrayRowColumnReverseColumns(T[][] data)
                : base(data)
            {
                Rows = data.GetLength(0);
                Columns = data.GetLength(1);
            }

            public T this[int row, int column]
            {
                get { return Data[row][Columns - 1 - column]; }
                set { Data[row][Columns - 1 - column] = value; }
            }
        }

        private class JaggedArrayRowColumnReverseRowsAndColumns<T> : AbstractJaggedArrayProxy<T>, IMatrixProxy<T>
        {
            public JaggedArrayRowColumnReverseRowsAndColumns(T[][] data)
                : base(data)
            {
                Rows = data.GetLength(0);
                Columns = data.GetLength(1);
            }

            public T this[int row, int column]
            {
                get { return Data[Rows - 1 - row][Columns - 1 - column]; }
                set { Data[Rows - 1 - row][Columns - 1 - column] = value; }
            }
        }

        private class JaggedArrayColumnRow<T> : AbstractJaggedArrayProxy<T>, IMatrixProxy<T>
        {
            public JaggedArrayColumnRow(T[][] data)
                : base(data)
            {
                Rows = data.GetLength(1);
                Columns = data.GetLength(0);
            }

            public T this[int row, int column]
            {
                get { return Data[column][row]; }
                set { Data[column][row] = value; }
            }
        }

        private class JaggedArrayColumnRowReverseRows<T> : AbstractJaggedArrayProxy<T>, IMatrixProxy<T>
        {
            public JaggedArrayColumnRowReverseRows(T[][] data)
                : base(data)
            {
                Rows = data.GetLength(1);
                Columns = data.GetLength(0);
            }

            public T this[int row, int column]
            {
                get { return Data[column][Rows - 1 - row]; }
                set { Data[column][Rows - 1 - row] = value; }
            }
        }

        private class JaggedArrayColumnRowReverseColumns<T> : AbstractJaggedArrayProxy<T>, IMatrixProxy<T>
        {
            public JaggedArrayColumnRowReverseColumns(T[][] data)
                : base(data)
            {
                Rows = data.GetLength(1);
                Columns = data.GetLength(0);
            }

            public T this[int row, int column]
            {
                get { return Data[Columns - 1 - column][row]; }
                set { Data[Columns - 1 - column][row] = value; }
            }
        }

        private class JaggedArrayColumnRowReverseRowsAndColumns<T> : AbstractJaggedArrayProxy<T>, IMatrixProxy<T>
        {
            public JaggedArrayColumnRowReverseRowsAndColumns(T[][] data)
                : base(data)
            {
                Rows = data.GetLength(1);
                Columns = data.GetLength(0);
            }

            public T this[int row, int column]
            {
                get { return Data[Columns - 1 - column][Rows - 1 - row]; }
                set { Data[Columns - 1 - column][Rows - 1 - row] = value; }
            }
        }

        #endregion

        #region Linear Array

        private class AbstractLinearArrayProxy<T> : BasicMatrixProxy, IEnumerable<T>
        {
            protected readonly T[] Data;
            protected readonly int Offset;

            public object RawData { get { return Data; } }

            protected AbstractLinearArrayProxy(T[] data, int offset, int rows, int columns)
            {
                if (offset + rows * columns > data.Length)
                {
                    throw new ArgumentException("Rows times columns is greater then the length of data.");
                }
                Rows = rows;
                Columns = columns;
                Data = data;
                Offset = offset;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<T> GetEnumerator()
            {
                var end = Offset + Rows * Columns;
                for (var i = Offset; i < end; i++)
                {
                    yield return Data[i];
                }
            }

            public void Apply(Func<T, T> f)
            {
                var end = Offset + Rows * Columns;
                for (var i = Offset; i < end; i++)
                {
                    Data[i] = f(Data[i]);
                }
            }

            public void Fill(T value)
            {
                var end = Offset + Rows * Columns;
                for (var i = Offset; i < end; i++)
                {
                    Data[i] = value;
                }
            }

            public void Fill(Func<T> f)
            {
                var end = Offset + Rows * Columns;
                for (var i = Offset; i < end; i++)
                {
                    Data[i] = f();
                }
            }

            public void ForEach(Action<T> action)
            {
                var end = Offset + Rows * Columns;
                for (var i = Offset; i < end; i++)
                {
                    action(Data[i]);
                }
            }
        }

        private class LinearArrayRowColumn<T> : AbstractLinearArrayProxy<T>, IMatrixProxy<T>
        {
            public LinearArrayRowColumn(T[] data, int offset, int rows, int columns)
                : base(data, offset, rows, columns)
            { }

            public T this[int row, int column]
            {
                get { return Data[Offset + row * Columns + column]; }
                set { Data[Offset + row * Columns + column] = value; }
            }
        }

        private class LinearArrayRowColumnReverseRows<T> : AbstractLinearArrayProxy<T>, IMatrixProxy<T>
        {
            public LinearArrayRowColumnReverseRows(T[] data, int offset, int rows, int columns)
                : base(data, offset, rows, columns)
            { }

            public T this[int row, int column]
            {
                get { return Data[Offset + (Rows - 1 - row) * Columns + column]; }
                set { Data[Offset + (Rows - 1 - row) * Columns + column] = value; }
            }
        }

        private class LinearArrayRowColumnReverseColumns<T> : AbstractLinearArrayProxy<T>, IMatrixProxy<T>
        {
            public LinearArrayRowColumnReverseColumns(T[] data, int offset, int rows, int columns)
                : base(data, offset, rows, columns)
            { }

            public T this[int row, int column]
            {
                get { return Data[Offset + row * Columns + (Columns - 1 - column)]; }
                set { Data[Offset + row * Columns + (Columns - 1 - column)] = value; }
            }
        }

        private class LinearArrayRowColumnReverseRowsAndColumns<T> : AbstractLinearArrayProxy<T>, IMatrixProxy<T>
        {
            public LinearArrayRowColumnReverseRowsAndColumns(T[] data, int offset, int rows, int columns)
                : base(data, offset, rows, columns)
            { }

            public T this[int row, int column]
            {
                get { return Data[Offset + (Rows - 1 - row) * Columns + (Columns - 1 - column)]; }
                set { Data[Offset + (Rows - 1 - row) * Columns + (Columns - 1 - column)] = value; }
            }
        }

        private class LinearArrayColumnRow<T> : AbstractLinearArrayProxy<T>, IMatrixProxy<T>
        {
            public LinearArrayColumnRow(T[] data, int offset, int rows, int columns)
                : base(data, offset, rows, columns)
            { }

            public T this[int row, int column]
            {
                get { return Data[Offset + column * Rows + row]; }
                set { Data[Offset + column * Rows + row] = value; }
            }
        }

        private class LinearArrayColumnRowReverseRows<T> : AbstractLinearArrayProxy<T>, IMatrixProxy<T>
        {
            public LinearArrayColumnRowReverseRows(T[] data, int offset, int rows, int columns)
                : base(data, offset, rows, columns)
            { }

            public T this[int row, int column]
            {
                get { return Data[Offset + column * Rows + (Rows - 1 - row)]; }
                set { Data[Offset + column * Rows + (Rows - 1 - row)] = value; }
            }
        }

        private class LinearArrayColumnRowReverseColumns<T> : AbstractLinearArrayProxy<T>, IMatrixProxy<T>
        {
            public LinearArrayColumnRowReverseColumns(T[] data, int offset, int rows, int columns)
                : base(data, offset, rows, columns)
            { }

            public T this[int row, int column]
            {
                get { return Data[Offset + (Columns - 1 - column) * Rows + row]; }
                set { Data[Offset + (Columns - 1 - column) * Rows + row] = value; }
            }
        }

        private class LinearArrayColumnRowReverseRowsAndColumns<T> : AbstractLinearArrayProxy<T>, IMatrixProxy<T>
        {
            public LinearArrayColumnRowReverseRowsAndColumns(T[] data, int offset, int rows, int columns)
                : base(data, offset, rows, columns)
            { }

            public T this[int row, int column]
            {
                get { return Data[Offset + (Columns - 1 - column) * Rows + (Rows - 1 - row)]; }
                set { Data[Offset + (Columns - 1 - column) * Rows + (Rows - 1 - row)] = value; }
            }
        }

        #endregion
    }

}