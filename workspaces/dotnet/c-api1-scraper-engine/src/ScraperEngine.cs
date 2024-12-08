using System;
using System.Diagnostics;
using System.Threading;

namespace OMP.LSWTSS.CApi1;

public static class ScraperEngine
{
    static AsIScriptEngine.Handle asIScriptEngineHandle;

    static readonly CFuncHook1<AsCScriptEngine.RegisterObjectTypeMethod.Delegate> asCScriptEngineRegisterObjectTypeMethodHook = new(
        AsCScriptEngine.RegisterObjectTypeMethod.Ptr,
        (
            AsCScriptEngine.Handle handle,
            string name,
            int byteSize,
            int flags
        ) =>
        {
            if (handle != nint.Zero && asIScriptEngineHandle == nint.Zero)
            {
                asIScriptEngineHandle = (AsIScriptEngine.Handle)(nint)handle;

                ThreadPool.QueueUserWorkItem(
                    (_) =>
                    {
                        Thread.Sleep(5000);

                        Console.WriteLine("Started scraping!");

                        try
                        {
                            ScrapeAngelScriptSchema.Execute(asIScriptEngineHandle);

                            Console.WriteLine("Finished scraping!");

                            Thread.Sleep(1000);

                            Process.GetCurrentProcess().Kill();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                );
            }

            return asCScriptEngineRegisterObjectTypeMethodHook!.Trampoline!(handle, name, byteSize, flags);
        }
    );

    public static int Init(IntPtr args, int sizeBytes)
    {
        asCScriptEngineRegisterObjectTypeMethodHook.Enable();

        return 0;
    }
}