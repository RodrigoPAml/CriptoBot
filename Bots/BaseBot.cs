namespace CriptoBOT.Bots
{
    /// <summary>
    /// Represents a recipee for bot implementations
    /// </summary>
    public abstract class BaseBot
    {
        public async Task Run()
        {
            try
            {
                while (true)
                {
                    if(!await Tick())
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bot failed to run: {ex}");
            }
        }

        /// <summary>
        /// Bot tick, to stop return false else return true
        /// </summary>
        /// <returns></returns>
        protected abstract Task<bool> Tick();
    }
}
