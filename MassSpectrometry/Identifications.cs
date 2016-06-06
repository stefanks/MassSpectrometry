using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassSpectrometry
{
    public interface Identifications
    {
         int getNumBelow(double thresholdPassParameter);

         bool isDecoy(int matchIndex);

         string spectrumID(int matchIndex);

         double calculatedMassToCharge(int matchIndex);

         double experimentalMassToCharge(int matchIndex);

         string PeptideSequence(int matchIndex);

         int chargeState(int matchIndex);

         int NumModifications(int matchIndex);

         int modificationLocation(int matchIndex, int i);

         string modificationDictionary(int matchIndex, int i);

         string modificationAcession(int matchIndex, int i);
    }
}
