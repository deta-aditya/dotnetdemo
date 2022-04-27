namespace TemperatureConverter.Programs.FauxionalProgram;

public abstract class Result<TLeft, TRight>
{
    public abstract Result<TLeftNew, TRight> MapError<TLeftNew>(Func<TLeft, TLeftNew> func);

    public abstract Result<TLeftNew, TRightNew> Bind<TLeftNew, TRightNew>(
        Func<TRight, Result<TLeftNew, TRightNew>> func);

    public abstract void Fork(Action<TRight> onOk, Action<TLeft> onError);

    public static Func<Result<TLeft, T1>, Result<TLeft, T2>, Result<TLeft, TRight>> Lift2<T1, T2>(
        Func<T1, T2, TRight> func)
    {
        return (result1, result2) => (result1, result2) switch
        {
            (Ok<TLeft, T1> ok1, Ok<TLeft, T2> ok2) => new Ok<TLeft, TRight>(func(ok1.Value, ok2.Value)),
            (Error<TLeft, T1> err, _) => new Error<TLeft, TRight>(err.Message),
            (_, Error<TLeft, T2> err) => new Error<TLeft, TRight>(err.Message),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static Func<Result<TLeft, T1>, Result<TLeft, T2>, Result<TLeft, T3>, Result<TLeft, TRight>>
        Lift3<T1, T2, T3>(
            Func<T1, T2, T3, TRight> func)
    {
        return (result1, result2, result3) => (result1, result2, result3) switch
        {
            (Ok<TLeft, T1> ok1, Ok<TLeft, T2> ok2, Ok<TLeft, T3> ok3) => new Ok<TLeft, TRight>(func(ok1.Value,
                ok2.Value, ok3.Value)),
            (Error<TLeft, T1> err, _, _) => new Error<TLeft, TRight>(err.Message),
            (_, Error<TLeft, T2> err, _) => new Error<TLeft, TRight>(err.Message),
            (_, _, Error<TLeft, T3> err) => new Error<TLeft, TRight>(err.Message),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static
        Func<Result<TLeft, T1>, Result<TLeft, T2>, Result<TLeft, T3>, Result<TLeft, T4>, Result<TLeft, TRight>> Lift4<
            T1, T2, T3, T4>(
            Func<T1, T2, T3, T4, TRight> func)
    {
        return (result1, result2, result3, result4) => (result1, result2, result3, result4) switch
        {
            (Ok<TLeft, T1> ok1, Ok<TLeft, T2> ok2, Ok<TLeft, T3> ok3, Ok<TLeft, T4> ok4) => new Ok<TLeft, TRight>(
                func(ok1.Value, ok2.Value, ok3.Value, ok4.Value)),
            (Error<TLeft, T1> err, _, _, _) => new Error<TLeft, TRight>(err.Message),
            (_, Error<TLeft, T2> err, _, _) => new Error<TLeft, TRight>(err.Message),
            (_, _, Error<TLeft, T3> err, _) => new Error<TLeft, TRight>(err.Message),
            (_, _, _, Error<TLeft, T4> err) => new Error<TLeft, TRight>(err.Message),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static Result<TLeft, TRight> FromNullable(TRight? nullable, TLeft valueIfNull)
    {
        return nullable switch
        {
            null => new Error<TLeft, TRight>(valueIfNull),
            _ => new Ok<TLeft, TRight>(nullable)
        };
    }

    public static Result<TLeft, TRight> Ok(TRight value)
    {
        return new Ok<TLeft, TRight>(value);
    }

    public static Result<TLeft, TRight> Error(TLeft message)
    {
        return new Error<TLeft, TRight>(message);
    }
}

public class Ok<TLeft, TRight> : Result<TLeft, TRight>
{
    public Ok(TRight value)
    {
        Value = value;
    }

    public TRight Value { get; }

    public override Result<TLeftNew, TRight> MapError<TLeftNew>(Func<TLeft, TLeftNew> func)
    {
        return new Ok<TLeftNew, TRight>(Value);
    }

    public override Result<TLeftNew, TRightNew> Bind<TLeftNew, TRightNew>(
        Func<TRight, Result<TLeftNew, TRightNew>> func)
    {
        return func(Value);
    }

    public override void Fork(Action<TRight> onOk, Action<TLeft> onError)
    {
        onOk(Value);
    }
}

public class Error<TLeft, TRight> : Result<TLeft, TRight>
{
    public Error(TLeft message)
    {
        Message = message;
    }

    public TLeft Message { get; }

    public override Result<TLeftNew, TRight> MapError<TLeftNew>(Func<TLeft, TLeftNew> func)
    {
        return new Error<TLeftNew, TRight>(func(Message));
    }

    public override Result<TLeftNew, TRightNew> Bind<TLeftNew, TRightNew>(
        Func<TRight, Result<TLeftNew, TRightNew>> func)
    {
        return new Error<TLeftNew, TRightNew>((TLeftNew)(object)Message!);
    }

    public override void Fork(Action<TRight> onOk, Action<TLeft> onError)
    {
        onError(Message);
    }
}