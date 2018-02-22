//-----------------------------------------------------------------------
// <copyright file="MatlabVizualizations.cs" company="Aaron Caffrey">
//     Copyright (c) 2013-2014 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection;
using BioinformaticsHelperLibrary.Measurements;
using BioinformaticsHelperLibrary.ProteinDataBankFormat;
using BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes;

namespace BioinformaticsHelperLibrary.Misc
{
    public class MatlabVisualizations
    {
        public static void SaveMatlabVizualization(List<string> matlabVizualization, string vizualizationOutputFilenameTemplate, string proteinId, string vizualisationId)
        {
            const string templateVariablePdbId = "%pdbid%";
            const string templateVariableVizualisationId = "%vizid%";

            if (matlabVizualization == null || matlabVizualization.Count == 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (string.IsNullOrWhiteSpace(vizualizationOutputFilenameTemplate))
            {
                throw new ArgumentOutOfRangeException(nameof(vizualizationOutputFilenameTemplate), vizualizationOutputFilenameTemplate, "Parameter was " + ParameterValidation.NullEmptyOrWhiteSpaceToString(vizualizationOutputFilenameTemplate));
            }

            if (!vizualizationOutputFilenameTemplate.Contains(templateVariablePdbId))
            {
                throw new ArgumentOutOfRangeException(nameof(vizualizationOutputFilenameTemplate));
            }

            if (!vizualizationOutputFilenameTemplate.Contains(templateVariableVizualisationId))
            {
                throw new ArgumentOutOfRangeException(nameof(vizualizationOutputFilenameTemplate));
            }

            if (string.IsNullOrWhiteSpace(proteinId))
            {
                throw new ArgumentOutOfRangeException();
            }

            vizualizationOutputFilenameTemplate = vizualizationOutputFilenameTemplate.Replace(templateVariablePdbId, proteinId);
            vizualizationOutputFilenameTemplate = vizualizationOutputFilenameTemplate.Replace(templateVariableVizualisationId, vizualisationId);

            string path = vizualizationOutputFilenameTemplate.Substring(0, vizualizationOutputFilenameTemplate.LastIndexOfAny(new[] {'\\', '/'}) + 1);
            string file = FileAndPathMethods.FullPathToFilename(vizualizationOutputFilenameTemplate, false).Replace(" ", "_");

            var fileInfo = new FileInfo(path + file);
            fileInfo.Directory.Create();
            File.WriteAllLines(fileInfo.FullName, matlabVizualization);
        }

        public static void SaveAllMatlabClusteringVisualizations(decimal minimumProteinInterfaceDensity, string pdbFilename, string proteinId, ProteinChainListContainer chainInteractingAtomLists, ClusteringFullResultListContainer clusters, string vizualizationOutputFilename)
        {
            if (string.IsNullOrWhiteSpace(vizualizationOutputFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(vizualizationOutputFilename));
            }

            if (string.IsNullOrWhiteSpace(pdbFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFilename));
            }

            if (string.IsNullOrWhiteSpace(proteinId))
            {
                proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);
            }

            SaveMatlabVizualization(GenerateMatlabClusteringVisualizationWithoutSubplot3D(minimumProteinInterfaceDensity, chainInteractingAtomLists, clusters, pdbFilename), vizualizationOutputFilename, proteinId, "3D Merged");
            SaveMatlabVizualization(GenerateMatlabClusteringVisualization1D(minimumProteinInterfaceDensity, chainInteractingAtomLists, clusters, pdbFilename), vizualizationOutputFilename, proteinId, "1D");
            SaveMatlabVizualization(GenerateMatlabClusteringVisualization3D(minimumProteinInterfaceDensity, chainInteractingAtomLists, clusters, pdbFilename), vizualizationOutputFilename, proteinId, "3D");
        }

        /// <summary>
        ///     This method generates a visualization in MATLAB code of the clustering algorithms output.
        /// </summary>
        /// <param name="chainInteractingAtomLists"></param>
        /// <param name="clustersStagesGroupsMembers"></param>
        /// <param name="proteinId"></param>
        /// <returns></returns>
        public static List<string> GenerateMatlabClusteringVisualizationWithoutSubplot3D(decimal minimumProteinInterfaceDensity, ProteinChainListContainer chainInteractingAtomLists, ClusteringFullResultListContainer clustersStagesGroupsMembers, string pdbFilename)
        {
            if (chainInteractingAtomLists == null || chainInteractingAtomLists.ChainList == null || chainInteractingAtomLists.ChainList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(chainInteractingAtomLists));
            }

            if (clustersStagesGroupsMembers == null || clustersStagesGroupsMembers.ChainList == null || clustersStagesGroupsMembers.ChainList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(clustersStagesGroupsMembers));
            }

            if (string.IsNullOrWhiteSpace(pdbFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFilename));
            }

            var output = new List<string>();
            Color color = Color.Yellow;
            string proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

            List<List<int>> chainStageProteinInterfaceCount;
            ClusteringFullResultListContainer proteinInterfaces = ProteinInterfaceDetection.DetectProteinInterfaces(proteinId, chainInteractingAtomLists, clustersStagesGroupsMembers, out chainStageProteinInterfaceCount, ClusteringProteinInterfaceDensityDetectionOptions.ResidueSequenceIndex, minimumProteinInterfaceDensity);
            int[] finalProteinInterfaceStages = ProteinInterfaceTreeOptimalStageDetection.FindFinalProteinInterfaceStageIndexes(chainInteractingAtomLists, clustersStagesGroupsMembers, proteinInterfaces, chainStageProteinInterfaceCount);

            output.Add("% Clustering of protein: " + proteinId);
            output.Add("% PDB file location: " + pdbFilename);
            output.Add("% File generated at: " + DateTime.Now);

            for (int chainIndex = 0; chainIndex < finalProteinInterfaceStages.Length; chainIndex++)
            {
                int finalProteinInterfaceStage = finalProteinInterfaceStages[chainIndex];
                output.Add("% Chain " + (chainIndex + 1) + " best stage is " + (finalProteinInterfaceStage + 1) + "/" + proteinInterfaces.ChainList[chainIndex].StageList.Count + " having " + proteinInterfaces.ChainList[chainIndex].StageList[finalProteinInterfaceStage].ClusterList.Count(proteinInterface => proteinInterface.AtomIndexList.Count > 0) + " proteinInterfaces.");
            }

            // One time commands.
            output.Add(string.Empty);
            output.Add("%%% tidy up matlab %%%");

            output.Add("clear all;");
            output.Add("close all;");
            output.Add("clc;");

            output.Add(string.Empty);
            output.Add("pauseDuration = 0.5;");
            output.Add("waitKeypress = false;");
            output.Add("saveGifAnimation = true;");
            output.Add("gifAnimationFilename = strcat(mfilename('fullpath'), '.gif');");

            output.Add(string.Empty);
            output.Add("%%% new figure %%%");
            output.Add("f = figure('Position', [1 1 1366 768]);");

            // Commands for each chain.
            output.Add(string.Empty);
            output.Add("view(3);");
            output.Add("xlabel('x');");
            output.Add("ylabel('y');");
            output.Add("zlabel('z');");
            output.Add("axis equal;");
            output.Add("title('');");
            output.Add("hold on;");
            output.Add("grid on;");

            output.Add(string.Empty);
            output.Add("%%% setting default colour %%%");
            output.Add("c = [" + Math.Round(color.R/255.0d, 2) + " " + Math.Round(color.G/255.0d, 2) + " " + Math.Round(color.B/255.0d, 2) + "];");

            var minmaxlist = new List<Point3D>();

            for (int clusterIndex = 0; clusterIndex < chainInteractingAtomLists.ChainList.Count; clusterIndex++)
            {
                minmaxlist.AddRange(GetMinMaxAxesFromPoints(Clustering.AtomRecordListToPoint3DList(chainInteractingAtomLists.ChainList[clusterIndex])).ToList());
            }

            Point3D[] minmax = GetMinMaxAxesFromPoints(minmaxlist);

            if (minmax[0].X >= minmax[1].X)
            {
                minmax[1].X = minmax[0].X + 0.1m;
            }

            if (minmax[0].Y >= minmax[1].Y)
            {
                minmax[1].Y = minmax[0].Y + 0.1m;
            }

            if (minmax[0].Z >= minmax[1].Z)
            {
                minmax[1].Z = minmax[0].Z + 0.1m;
            }

            output.Add(string.Empty);
            output.Add("axis([" + minmax[0].X + " " + minmax[1].X + " " + minmax[0].Y + " " + minmax[1].Y + " " + minmax[0].Z + " " + minmax[1].Z + "]);");

            for (int clusterIndex = 0; clusterIndex < chainInteractingAtomLists.ChainList.Count; clusterIndex++)
            {
                // Draw all text boxes - in the initial colour.
                output.Add(string.Empty);
                output.Add("%%% drawing text boxes for chain " + (clusterIndex + 1) + " %%%");
                for (int memberIndex = 0; memberIndex < chainInteractingAtomLists.ChainList[clusterIndex].AtomList.Count; memberIndex++)
                {
                    ATOM_Record atom = chainInteractingAtomLists.ChainList[clusterIndex].AtomList[memberIndex];
                    output.Add("t(" + (clusterIndex + 1) + ", " + (memberIndex + 1) + ") = text(" + atom.x.FieldValue + ", " + atom.y.FieldValue + ", " + atom.z.FieldValue + ", '" + atom.resSeq.FieldValue + "', 'BackgroundColor', c, 'Color', [1 1 1]);");
                }

                // Draw all possible lines - and make invisible.
                output.Add(string.Empty);
                output.Add("%%% drawing lines for chain " + (clusterIndex + 1) + " %%%");
                for (int memberIndex1 = 0; memberIndex1 < chainInteractingAtomLists.ChainList[clusterIndex].AtomList.Count; memberIndex1++)
                {
                    var point1 = new Point3D(chainInteractingAtomLists.ChainList[clusterIndex].AtomList[memberIndex1].x.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[memberIndex1].y.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[memberIndex1].z.FieldValue);

                    for (int memberIndex2 = 0; memberIndex2 < chainInteractingAtomLists.ChainList[clusterIndex].AtomList.Count; memberIndex2++)
                    {
                        var point2 = new Point3D(chainInteractingAtomLists.ChainList[clusterIndex].AtomList[memberIndex2].x.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[memberIndex2].y.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[memberIndex2].z.FieldValue);

                        output.Add("l(" + (clusterIndex + 1) + ", " + (memberIndex1 + 1) + ", " + (memberIndex2 + 1) + ") = line([" + point1.X + " " + point2.X + "], [" + point1.Y + " " + point2.Y + "], [" + point1.Z + " " + point2.Z + "], 'Color', c, 'Visible', 'off', 'LineWidth', 2);");
                    }
                }
            }

            output.Add(string.Empty);
            output.Add("%%% saving frame %%%");
            output.Add("drawnow();");

            output.Add("if saveGifAnimation == true");
            output.Add("    [imind, cm] = rgb2ind(frame2im(getframe(f)), 256);");
            output.Add("    imwrite(imind, cm, gifAnimationFilename, 'Loopcount', inf);");
            output.Add("end;");

            output.Add("if pauseDuration > 0");
            output.Add("    pause(pauseDuration);");
            output.Add("end;");

            output.Add("if waitKeypress == true");
            output.Add("    waitforbuttonpress;");
            output.Add("end;");

            var clusterOutput = new List<List<string>>();
            int clusterOutputIndex;

            for (int clusterIndex = 0; clusterIndex < clustersStagesGroupsMembers.ChainList.Count; clusterIndex++)
            {
                clusterOutputIndex = clusterIndex;

                for (int stageIndex = 0; stageIndex < clustersStagesGroupsMembers.ChainList[clusterIndex].StageList.Count; stageIndex++)
                {
                    if (stageIndex > finalProteinInterfaceStages[clusterIndex])
                    {
                        break;
                    }

                    while (clusterOutputIndex > clusterOutput.Count - 1)
                    {
                        clusterOutput.Add(new List<string>());
                    }

                    clusterOutput[clusterOutputIndex].Add(string.Empty);
                    clusterOutput[clusterOutputIndex].Add("%%% drawing cluster " + (clusterIndex + 1) + " %%%");

                    clusterOutput[clusterOutputIndex].Add(string.Empty);
                    clusterOutput[clusterOutputIndex].Add("%%% drawing stage " + (clusterIndex + 1) + "." + (stageIndex + 1) + " %%%");
                    for (int groupIndex = 0; groupIndex < clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList.Count; groupIndex++)
                    {
                        clusterOutput[clusterOutputIndex].Add(string.Empty);
                        clusterOutput[clusterOutputIndex].Add("%%% drawing group " + (clusterIndex + 1) + "." + (stageIndex + 1) + "." + (groupIndex + 1) + " %%%");
                        color = GetGroupColor(((clusterIndex + 1) ^ (groupIndex + 1)) - 1);

                        clusterOutput[clusterOutputIndex].Add("c = [" + Math.Round(color.R/255.0d, 2) + " " + Math.Round(color.G/255.0d, 2) + " " + Math.Round(color.B/255.0d, 2) + "];");

                        string memberStr = string.Empty;

                        for (int memberIndex1 = 0; memberIndex1 < clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList.Count; memberIndex1++)
                        {
                            var point1 = new Point3D(chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1]].x.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1]].y.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1]].z.FieldValue);

                            memberStr += chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1]].resSeq.FieldValue + " ";
                            for (int memberIndex2 = 0; memberIndex2 < clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList.Count; memberIndex2++)
                            {
                                var point2 = new Point3D(chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex2]].x.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex2]].y.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex2]].z.FieldValue);

                                clusterOutput[clusterOutputIndex].Add("set(l(" + (clusterIndex + 1) + ", " + (clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1] + 1) + ", " + (clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex2] + 1) + "), 'Color', c, 'Visible', 'on');");
                            }

                            clusterOutput[clusterOutputIndex].Add("set(t(" + (clusterIndex + 1) + ", " + (clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1] + 1) + "), 'BackgroundColor', c, 'Color', [1 1 1]);");
                        }

                        clusterOutput[clusterOutputIndex].Add("title('[Cluster " + clusterIndex + "] [Stage " + stageIndex + "] [Group " + groupIndex + "]: " + memberStr + "');");
                    }

                    clusterOutput[clusterOutputIndex].Add("if pauseDuration > 0");
                    clusterOutput[clusterOutputIndex].Add("    pause(pauseDuration);");
                    clusterOutput[clusterOutputIndex].Add("end;");

                    clusterOutput[clusterOutputIndex].Add("if waitKeypress == true");
                    clusterOutput[clusterOutputIndex].Add("    waitforbuttonpress;");
                    clusterOutput[clusterOutputIndex].Add("end;");

                    clusterOutputIndex += clustersStagesGroupsMembers.ChainList.Count;
                }
            }

            for (int index = 0; index < clusterOutput.Count; index++)
            {
                output.AddRange(clusterOutput[index]);

                if ((index + 1)%clustersStagesGroupsMembers.ChainList.Count == 0)
                {
                    output.Add(string.Empty);
                    output.Add("if saveGifAnimation == true");
                    output.Add("    %%% saving frame %%%");
                    output.Add("    drawnow();");
                    output.Add("    [imind, cm] = rgb2ind(frame2im(getframe(f)), 256);");
                    output.Add("    imwrite(imind, cm, gifAnimationFilename, 'WriteMode', 'append');");
                    output.Add("end;");
                }
            }

            return output;
        }

        public static Point1D[] GetMinMaxFromResidueSequenceNumber(ProteinAtomListContainer proteinAtomListContainer)
        {
            if (proteinAtomListContainer == null || proteinAtomListContainer.AtomList == null || proteinAtomListContainer.AtomList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(proteinAtomListContainer));
            }

            var min = new Point1D();
            var max = new Point1D();

            for (int index = 0; index < proteinAtomListContainer.AtomList.Count; index++)
            {
                ATOM_Record atom = proteinAtomListContainer.AtomList[index];
                int p;

                if (!int.TryParse(atom.resSeq.FieldValue, out p))
                {
                    continue;
                }

                if (index == 0)
                {
                    min.P = p;
                    max.P = p;
                }
                else
                {
                    if (p > max.P)
                    {
                        max.P = p;
                    }

                    if (p < min.P)
                    {
                        min.P = p;
                    }
                }
            }

            return new[] {min, max};
        }

        /// <summary>
        ///     This method generates a visualization in MATLAB code of the clustering algorithms output.
        /// </summary>
        /// <param name="chainInteractingAtomLists"></param>
        /// <param name="clustersStagesGroupsMembers"></param>
        /// <param name="proteinId"></param>
        /// <returns></returns>
        public static List<string> GenerateMatlabClusteringVisualization1D(decimal minimumProteinInterfaceDensity, ProteinChainListContainer chainInteractingAtomLists, ClusteringFullResultListContainer clustersStagesGroupsMembers, string pdbFilename)
        {
            if (chainInteractingAtomLists == null || chainInteractingAtomLists.ChainList == null || chainInteractingAtomLists.ChainList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(chainInteractingAtomLists));
            }

            if (clustersStagesGroupsMembers == null || clustersStagesGroupsMembers.ChainList == null || clustersStagesGroupsMembers.ChainList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(clustersStagesGroupsMembers));
            }

            if (string.IsNullOrWhiteSpace(pdbFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFilename));
            }

            var output = new List<string>();
            Color color = Color.Yellow;

            string proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

            List<List<int>> chainStageProteinInterfaceCount;
            ClusteringFullResultListContainer proteinInterfaces = ProteinInterfaceDetection.DetectProteinInterfaces(proteinId, chainInteractingAtomLists, clustersStagesGroupsMembers, out chainStageProteinInterfaceCount,ClusteringProteinInterfaceDensityDetectionOptions.ResidueSequenceIndex, minimumProteinInterfaceDensity);
            int[] finalProteinInterfaceStages = ProteinInterfaceTreeOptimalStageDetection.FindFinalProteinInterfaceStageIndexes(chainInteractingAtomLists, clustersStagesGroupsMembers, proteinInterfaces, chainStageProteinInterfaceCount);

            output.Add("% Clustering of protein: " + proteinId);
            output.Add("% PDB file location: " + pdbFilename);
            output.Add("% File generated at: " + DateTime.Now);

            for (int chainIndex = 0; chainIndex < finalProteinInterfaceStages.Length; chainIndex++)
            {
                int finalProteinInterfaceStage = finalProteinInterfaceStages[chainIndex];
                output.Add("% Chain " + (chainIndex + 1) + " best stage is " + (finalProteinInterfaceStage + 1) + "/" + proteinInterfaces.ChainList[chainIndex].StageList.Count + " having " + proteinInterfaces.ChainList[chainIndex].StageList[finalProteinInterfaceStage].ClusterList.Count(proteinInterface => proteinInterface.AtomIndexList.Count > 0) + " proteinInterfaces.");
            }

            // One time commands.
            output.Add(string.Empty);
            output.Add("%%% tidy up matlab %%%");

            output.Add("clear all;");
            output.Add("close all;");
            output.Add("clc;");

            output.Add(string.Empty);
            output.Add("pauseDuration = 0.5;");
            output.Add("waitKeypress = false;");
            output.Add("saveGifAnimation = true;");
            output.Add("gifAnimationFilename = strcat(mfilename('fullpath'), '.gif');");

            output.Add(string.Empty);
            output.Add("%%% new figure %%%");
            output.Add("f = figure('Position', [1 1 1366 768]);");

            // Commands for each chain.
            for (int subplot = 1; subplot <= 2; subplot++)
            {
                output.Add(string.Empty);
                output.Add("%%% make subplot s(" + subplot + ") and configure %%%");
                output.Add("s(" + subplot + ") = subplot(2,1," + subplot + ");");
                output.Add("view(2);");
                output.Add("xlabel('x');");
                output.Add("ylabel('y');");
                output.Add("axis equal;");
                output.Add("title('chain " + subplot + "');");
                output.Add("hold on;");
                output.Add("grid on;");
            }

            output.Add(string.Empty);
            output.Add("%%% setting default colour %%%");
            output.Add("c = [" + Math.Round(color.R/255.0d, 2) + " " + Math.Round(color.G/255.0d, 2) + " " + Math.Round(color.B/255.0d, 2) + "];");

            for (int clusterIndex = 0; clusterIndex < chainInteractingAtomLists.ChainList.Count; clusterIndex++)
            {
                Point1D[] minmax = GetMinMaxFromResidueSequenceNumber(chainInteractingAtomLists.ChainList[clusterIndex]);

                if (minmax[0].P >= minmax[1].P)
                {
                    minmax[1].P = minmax[0].P + 0.1m;
                }

                output.Add(string.Empty);
                output.Add("%%% setting axis for subplot s(" + (clusterIndex + 1) + ") %%%");
                output.Add("subplot(s(" + (clusterIndex + 1) + "));");
                output.Add("axis([" + minmax[0].P + " " + minmax[1].P + " -1 1 -1 1]);");

                // Draw all text boxes - in the initial colour.
                output.Add(string.Empty);
                output.Add("%%% drawing text boxes for subplot s(" + (clusterIndex + 1) + ") %%%");
                for (int memberIndex = 0; memberIndex < chainInteractingAtomLists.ChainList[clusterIndex].AtomList.Count; memberIndex++)
                {
                    ATOM_Record atom = chainInteractingAtomLists.ChainList[clusterIndex].AtomList[memberIndex];
                    output.Add("t(" + (clusterIndex + 1) + ", " + (memberIndex + 1) + ") = text(" + atom.resSeq.FieldValue + ", 0, 0, '" + atom.resSeq.FieldValue + "', 'BackgroundColor', c, 'Color', [1 1 1]);");
                }

                // Draw all possible lines - and make invisible.
                output.Add(string.Empty);
                output.Add("%%% drawing lines for subplot s(" + (clusterIndex + 1) + ") %%%");
                for (int memberIndex1 = 0; memberIndex1 < chainInteractingAtomLists.ChainList[clusterIndex].AtomList.Count; memberIndex1++)
                {
                    var point1 = PointConversions.AtomResidueSequencePoint1D(chainInteractingAtomLists.ChainList[clusterIndex].AtomList[memberIndex1]);

                    for (int memberIndex2 = 0; memberIndex2 < chainInteractingAtomLists.ChainList[clusterIndex].AtomList.Count; memberIndex2++)
                    {
                        var point2 = PointConversions.AtomResidueSequencePoint1D(chainInteractingAtomLists.ChainList[clusterIndex].AtomList[memberIndex2]);

                        if (memberIndex1 != memberIndex2)
                        {
                            output.Add("l(" + (clusterIndex + 1) + ", " + (memberIndex1 + 1) + ", " + (memberIndex2 + 1) + ") = line([" + point1.P + " " + point2.P + "], [0 0], [0 0], 'Color', c, 'Visible', 'off', 'LineWidth', 2);");
                        }
                    }
                }
            }

            output.Add(string.Empty);
            output.Add("%%% saving frame %%%");
            output.Add("drawnow();");

            output.Add("if saveGifAnimation == true");
            output.Add("    [imind, cm] = rgb2ind(frame2im(getframe(f)), 256);");
            output.Add("    imwrite(imind, cm, gifAnimationFilename, 'Loopcount', inf);");
            output.Add("end;");

            output.Add("if pauseDuration > 0");
            output.Add("    pause(pauseDuration);");
            output.Add("end;");

            output.Add("if waitKeypress == true");
            output.Add("    waitforbuttonpress;");
            output.Add("end;");

            var clusterOutput = new List<List<string>>();
            int clusterOutputIndex;

            for (int clusterIndex = 0; clusterIndex < clustersStagesGroupsMembers.ChainList.Count; clusterIndex++)
            {
                clusterOutputIndex = clusterIndex;

                for (int stageIndex = 0; stageIndex < clustersStagesGroupsMembers.ChainList[clusterIndex].StageList.Count; stageIndex++)
                {
                    if (stageIndex > finalProteinInterfaceStages[clusterIndex])
                    {
                        break;
                    }

                    while (clusterOutputIndex > clusterOutput.Count - 1)
                    {
                        clusterOutput.Add(new List<string>());
                    }

                    clusterOutput[clusterOutputIndex].Add(string.Empty);
                    clusterOutput[clusterOutputIndex].Add("%%% drawing cluster " + (clusterIndex + 1) + " %%%");
                    clusterOutput[clusterOutputIndex].Add("subplot(s(" + (clusterIndex + 1) + "));");

                    clusterOutput[clusterOutputIndex].Add(string.Empty);
                    clusterOutput[clusterOutputIndex].Add("%%% drawing stage " + (clusterIndex + 1) + "." + (stageIndex + 1) + " %%%");
                    for (int groupIndex = 0; groupIndex < clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList.Count; groupIndex++)
                    {
                        clusterOutput[clusterOutputIndex].Add(string.Empty);
                        clusterOutput[clusterOutputIndex].Add("%%% drawing group " + (clusterIndex + 1) + "." + (stageIndex + 1) + "." + (groupIndex + 1) + " %%%");
                        color = GetGroupColor(groupIndex);

                        clusterOutput[clusterOutputIndex].Add("c = [" + Math.Round(color.R/255.0d, 2) + " " + Math.Round(color.G/255.0d, 2) + " " + Math.Round(color.B/255.0d, 2) + "];");

                        string memberStr = string.Empty;

                        for (int memberIndex1 = 0; memberIndex1 < clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList.Count; memberIndex1++)
                        {
                            var point1 = new Point3D(chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1]].x.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1]].y.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1]].z.FieldValue);

                            memberStr += chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1]].resSeq.FieldValue + " ";
                            for (int memberIndex2 = 0; memberIndex2 < clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList.Count; memberIndex2++)
                            {
                                var point2 = new Point3D(chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex2]].x.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex2]].y.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex2]].z.FieldValue);

                                if (memberIndex1 != memberIndex2)
                                {
                                    clusterOutput[clusterOutputIndex].Add("set(l(" + (clusterIndex + 1) + ", " + (clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1] + 1) + ", " + (clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex2] + 1) + "), 'Color', c, 'Visible', 'on');");
                                }
                            }

                            clusterOutput[clusterOutputIndex].Add("set(t(" + (clusterIndex + 1) + ", " + (clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1] + 1) + "), 'BackgroundColor', c, 'Color', [1 1 1]);");
                        }

                        clusterOutput[clusterOutputIndex].Add("title('[Cluster " + clusterIndex + "] [Stage " + stageIndex + "] [Group " + groupIndex + "]: " + memberStr + "');");
                    }

                    clusterOutput[clusterOutputIndex].Add("if pauseDuration > 0");
                    clusterOutput[clusterOutputIndex].Add("    pause(pauseDuration);");
                    clusterOutput[clusterOutputIndex].Add("end;");
                    clusterOutput[clusterOutputIndex].Add("if waitKeypress == true");
                    clusterOutput[clusterOutputIndex].Add("    waitforbuttonpress;");
                    clusterOutput[clusterOutputIndex].Add("end;");

                    clusterOutputIndex += clustersStagesGroupsMembers.ChainList.Count;
                }
            }

            for (int index = 0; index < clusterOutput.Count; index++)
            {
                output.AddRange(clusterOutput[index]);

                if ((index + 1)%clustersStagesGroupsMembers.ChainList.Count == 0)
                {
                    output.Add(string.Empty);
                    output.Add("if saveGifAnimation == true");
                    output.Add("    %%% saving frame %%%");
                    output.Add("    drawnow();");
                    output.Add("    [imind, cm] = rgb2ind(frame2im(getframe(f)), 256);");
                    output.Add("    imwrite(imind, cm, gifAnimationFilename, 'WriteMode', 'append');");
                    output.Add("end;");
                }
            }

            return output;
        }

        /// <summary>
        ///     This method generates a visualization in MATLAB code of the clustering algorithms output.
        /// </summary>
        /// <param name="chainInteractingAtomLists"></param>
        /// <param name="clustersStagesGroupsMembers"></param>
        /// <param name="proteinId"></param>
        /// <returns></returns>
        public static List<string> GenerateMatlabClusteringVisualization3D(decimal minimumProteinInterfaceDensity, ProteinChainListContainer chainInteractingAtomLists, ClusteringFullResultListContainer clustersStagesGroupsMembers, string pdbFilename)
        {
            if (chainInteractingAtomLists == null || chainInteractingAtomLists.ChainList == null || chainInteractingAtomLists.ChainList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(chainInteractingAtomLists));
            }

            if (clustersStagesGroupsMembers == null || clustersStagesGroupsMembers.ChainList == null || clustersStagesGroupsMembers.ChainList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(clustersStagesGroupsMembers));
            }

            if (string.IsNullOrWhiteSpace(pdbFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFilename));
            }

            var output = new List<string>();
            Color color = Color.Yellow;
            string proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

            List<List<int>> chainStageProteinInterfaceCount;
            ClusteringFullResultListContainer proteinInterfaces = ProteinInterfaceDetection.DetectProteinInterfaces(proteinId, chainInteractingAtomLists, clustersStagesGroupsMembers, out chainStageProteinInterfaceCount, ClusteringProteinInterfaceDensityDetectionOptions.ResidueSequenceIndex, minimumProteinInterfaceDensity);
            int[] finalProteinInterfaceStages = ProteinInterfaceTreeOptimalStageDetection.FindFinalProteinInterfaceStageIndexes(chainInteractingAtomLists, clustersStagesGroupsMembers, proteinInterfaces, chainStageProteinInterfaceCount);

            output.Add("% Clustering of protein: " + proteinId);
            output.Add("% PDB file location: " + pdbFilename);
            output.Add("% File generated at: " + DateTime.Now);

            // note: finalProteinInterfaceStages.Lenth will be equal to the number of chains
            for (int index = 0; index < finalProteinInterfaceStages.Length; index++)
            {
                int finalProteinInterfaceStage = finalProteinInterfaceStages[index];
                output.Add("% Chain " + (index + 1) + " best stage is " + (finalProteinInterfaceStage + 1) + "/" + proteinInterfaces.ChainList[index].StageList.Count + " having " + proteinInterfaces.ChainList[index].StageList[finalProteinInterfaceStage].ClusterList.Count(proteinInterface => proteinInterface.AtomIndexList.Count > 0) + " proteinInterfaces.");
            }

            // One time commands.
            output.Add(string.Empty);
            output.Add("%%% tidy up matlab %%%");

            output.Add("clear all;");
            output.Add("close all;");
            output.Add("clc;");

            output.Add(string.Empty);
            output.Add("pauseDuration = 0.5;");
            output.Add("waitKeypress = false;");
            output.Add("saveGifAnimation = true;");
            output.Add("gifAnimationFilename = strcat(mfilename('fullpath'), '.gif');");

            output.Add(string.Empty);
            output.Add("%%% new figure %%%");
            output.Add("f = figure('Position', [1 1 1366 768]);");

            // Commands for each chain.
            for (int subplot = 1; subplot <= 2; subplot++)
            {
                output.Add(string.Empty);
                output.Add("%%% make subplot s(" + subplot + ") and configure %%%");
                output.Add("s(" + subplot + ") = subplot(2,1," + subplot + ");");
                output.Add("view(3);");
                output.Add("xlabel('x');");
                output.Add("ylabel('y');");
                output.Add("zlabel('z');");
                output.Add("axis equal;");
                output.Add("title('chain " + subplot + "');");
                output.Add("hold on;");
                output.Add("grid on;");
            }

            output.Add(string.Empty);
            output.Add("%%% setting default colour %%%");
            output.Add("c = [" + Math.Round(color.R/255.0d, 2) + " " + Math.Round(color.G/255.0d, 2) + " " + Math.Round(color.B/255.0d, 2) + "];");

            for (int clusterIndex = 0; clusterIndex < chainInteractingAtomLists.ChainList.Count; clusterIndex++)
            {
                Point3D[] minmax = GetMinMaxAxesFromPoints(Clustering.AtomRecordListToPoint3DList(chainInteractingAtomLists.ChainList[clusterIndex]));

                if (minmax[0].X >= minmax[1].X)
                {
                    minmax[1].X = minmax[0].X + 0.1m;
                }

                if (minmax[0].Y >= minmax[1].Y)
                {
                    minmax[1].Y = minmax[0].Y + 0.1m;
                }

                if (minmax[0].Z >= minmax[1].Z)
                {
                    minmax[1].Z = minmax[0].Z + 0.1m;
                }

                output.Add(string.Empty);
                output.Add("%%% setting axis for subplot s(" + (clusterIndex + 1) + ") %%%");
                output.Add("subplot(s(" + (clusterIndex + 1) + "));");
                output.Add("axis([" + minmax[0].X + " " + minmax[1].X + " " + minmax[0].Y + " " + minmax[1].Y + " " + minmax[0].Z + " " + minmax[1].Z + "]);");

                // Draw all text boxes - in the initial colour.
                output.Add(string.Empty);
                output.Add("%%% drawing text boxes for subplot s(" + (clusterIndex + 1) + ") %%%");
                for (int memberIndex = 0; memberIndex < chainInteractingAtomLists.ChainList[clusterIndex].AtomList.Count; memberIndex++)
                {
                    ATOM_Record atom = chainInteractingAtomLists.ChainList[clusterIndex].AtomList[memberIndex];
                    output.Add("t(" + (clusterIndex + 1) + ", " + (memberIndex + 1) + ") = text(" + atom.x.FieldValue + ", " + atom.y.FieldValue + ", " + atom.z.FieldValue + ", '" + atom.resSeq.FieldValue + "', 'BackgroundColor', c, 'Color', [1 1 1]);");
                }

                // Draw all possible lines - and make invisible.
                output.Add(string.Empty);
                output.Add("%%% drawing lines for subplot s(" + (clusterIndex + 1) + ") %%%");
                for (int memberIndex1 = 0; memberIndex1 < chainInteractingAtomLists.ChainList[clusterIndex].AtomList.Count; memberIndex1++)
                {
                    var point1 = new Point3D(chainInteractingAtomLists.ChainList[clusterIndex].AtomList[memberIndex1].x.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[memberIndex1].y.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[memberIndex1].z.FieldValue);

                    for (int memberIndex2 = 0; memberIndex2 < chainInteractingAtomLists.ChainList[clusterIndex].AtomList.Count; memberIndex2++)
                    {
                        var point2 = new Point3D(chainInteractingAtomLists.ChainList[clusterIndex].AtomList[memberIndex2].x.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[memberIndex2].y.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[memberIndex2].z.FieldValue);

                        if (memberIndex1 != memberIndex2)
                        {
                            output.Add("l(" + (clusterIndex + 1) + ", " + (memberIndex1 + 1) + ", " + (memberIndex2 + 1) + ") = line([" + point1.X + " " + point2.X + "], [" + point1.Y + " " + point2.Y + "], [" + point1.Z + " " + point2.Z + "], 'Color', c, 'Visible', 'off', 'LineWidth', 2);");
                        }
                    }
                }
            }

            output.Add(string.Empty);
            output.Add("%%% saving frame %%%");
            output.Add("drawnow();");

            output.Add("if saveGifAnimation == true");
            output.Add("    [imind, cm] = rgb2ind(frame2im(getframe(f)), 256);");
            output.Add("    imwrite(imind, cm, gifAnimationFilename, 'Loopcount', inf);");
            output.Add("end;");

            output.Add("if pauseDuration > 0");
            output.Add("    pause(pauseDuration);");
            output.Add("end;");

            output.Add("if waitKeypress == true");
            output.Add("    waitforbuttonpress;");
            output.Add("end;");

            var clusterOutput = new List<List<string>>();
            int clusterOutputIndex;

            for (int clusterIndex = 0; clusterIndex < clustersStagesGroupsMembers.ChainList.Count; clusterIndex++)
            {
                clusterOutputIndex = clusterIndex;

                for (int stageIndex = 0; stageIndex < clustersStagesGroupsMembers.ChainList[clusterIndex].StageList.Count; stageIndex++)
                {
                    while (clusterOutputIndex > clusterOutput.Count - 1)
                    {
                        clusterOutput.Add(new List<string>());
                    }

                    clusterOutput[clusterOutputIndex].Add(string.Empty);
                    clusterOutput[clusterOutputIndex].Add("%%% drawing cluster " + (clusterIndex + 1) + " %%%");
                    clusterOutput[clusterOutputIndex].Add("subplot(s(" + (clusterIndex + 1) + "));");

                    clusterOutput[clusterOutputIndex].Add(string.Empty);
                    clusterOutput[clusterOutputIndex].Add("%%% drawing stage " + (clusterIndex + 1) + "." + (stageIndex + 1) + " %%%");
                    for (int groupIndex = 0; groupIndex < clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList.Count; groupIndex++)
                    {
                        clusterOutput[clusterOutputIndex].Add(string.Empty);
                        clusterOutput[clusterOutputIndex].Add("%%% drawing group " + (clusterIndex + 1) + "." + (stageIndex + 1) + "." + (groupIndex + 1) + " %%%");
                        color = GetGroupColor(groupIndex);

                        clusterOutput[clusterOutputIndex].Add("c = [" + Math.Round(color.R/255.0d, 2) + " " + Math.Round(color.G/255.0d, 2) + " " + Math.Round(color.B/255.0d, 2) + "];");

                        string memberStr = string.Empty;

                        for (int memberIndex1 = 0; memberIndex1 < clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList.Count; memberIndex1++)
                        {
                            var point1 = new Point3D(chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1]].x.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1]].y.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1]].z.FieldValue);

                            memberStr += chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1]].resSeq.FieldValue + " ";
                            for (int memberIndex2 = 0; memberIndex2 < clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList.Count; memberIndex2++)
                            {
                                var point2 = new Point3D(chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex2]].x.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex2]].y.FieldValue, chainInteractingAtomLists.ChainList[clusterIndex].AtomList[clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex2]].z.FieldValue);

                                if (memberIndex1 != memberIndex2)
                                {
                                    clusterOutput[clusterOutputIndex].Add("set(l(" + (clusterIndex + 1) + ", " + (clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1] + 1) + ", " + (clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex2] + 1) + "), 'Color', c, 'Visible', 'on');");
                                }
                            }

                            clusterOutput[clusterOutputIndex].Add("set(t(" + (clusterIndex + 1) + ", " + (clustersStagesGroupsMembers.ChainList[clusterIndex].StageList[stageIndex].ClusterList[groupIndex].AtomIndexList[memberIndex1] + 1) + "), 'BackgroundColor', c, 'Color', [1 1 1]);");
                        }

                        clusterOutput[clusterOutputIndex].Add("title('[Cluster " + clusterIndex + "] [Stage " + stageIndex + "] [Group " + groupIndex + "]: " + memberStr + "');");
                    }

                    clusterOutput[clusterOutputIndex].Add("if pauseDuration > 0");
                    clusterOutput[clusterOutputIndex].Add("    pause(pauseDuration);");
                    clusterOutput[clusterOutputIndex].Add("end;");

                    clusterOutput[clusterOutputIndex].Add("if waitKeypress == true");
                    clusterOutput[clusterOutputIndex].Add("    waitforbuttonpress;");
                    clusterOutput[clusterOutputIndex].Add("end;");

                    clusterOutputIndex += clustersStagesGroupsMembers.ChainList.Count;
                }
            }

            for (int index = 0; index < clusterOutput.Count; index++)
            {
                output.AddRange(clusterOutput[index]);

                if ((index + 1)%clustersStagesGroupsMembers.ChainList.Count == 0)
                {
                    output.Add(string.Empty);
                    output.Add("if saveGifAnimation == true");
                    output.Add("    %%% saving frame %%%");
                    output.Add("    drawnow();");
                    output.Add("    [imind, cm] = rgb2ind(frame2im(getframe(f)), 256);");
                    output.Add("    imwrite(imind, cm, gifAnimationFilename, 'WriteMode', 'append');");
                    output.Add("end;");
                }
            }

            return output;
        }

        /// <summary>
        ///     This method finds both the minimum and maximum for x, y and z in a list of points.  These values can the be used to
        ///     specify the exact right size for axes on graphs representing these points.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Point3D[] GetMinMaxAxesFromPoints(List<Point3D> points)
        {
            if (points == null)
            {
                throw new ArgumentNullException(nameof(points));
            }

            if (points.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(points));
            }

            var min = new Point3D();
            var max = new Point3D();

            for (int pointsIndex = 0; pointsIndex < points.Count; pointsIndex++)
            {
                if (pointsIndex == 0)
                {
                    max.X = points[pointsIndex].X;
                    max.Y = points[pointsIndex].Y;
                    max.Z = points[pointsIndex].Z;

                    min.X = points[pointsIndex].X;
                    min.Y = points[pointsIndex].Y;
                    min.Z = points[pointsIndex].Z;
                }
                else
                {
                    if (points[pointsIndex].X > max.X)
                    {
                        max.X = points[pointsIndex].X;
                    }

                    if (points[pointsIndex].Y > max.Y)
                    {
                        max.Y = points[pointsIndex].Y;
                    }

                    if (points[pointsIndex].Z > max.Z)
                    {
                        max.Z = points[pointsIndex].Z;
                    }

                    if (points[pointsIndex].X < min.X)
                    {
                        min.X = points[pointsIndex].X;
                    }

                    if (points[pointsIndex].Y < min.Y)
                    {
                        min.Y = points[pointsIndex].Y;
                    }

                    if (points[pointsIndex].Z < min.Z)
                    {
                        min.Z = points[pointsIndex].Z;
                    }
                }
            }

            return new[] {min, max};
        }

        /// <summary>
        ///     This method returns a color for a given group number.
        /// </summary>
        /// <param name="groupNumber"></param>
        /// <returns></returns>
        public static Color GetGroupColor(int groupNumber)
        {
            if (groupNumber < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(groupNumber), groupNumber, "Parameter must be more than or equal to zero");
            }

            Color color;

            switch (groupNumber%19)
            {
                case 0:
                    color = Color.FromArgb(128, 0, 0);
                    break;
                case 1:
                    color = Color.FromArgb(0, 128, 0);
                    break;
                case 2:
                    color = Color.FromArgb(0, 0, 128);
                    break;
                case 3:
                    color = Color.FromArgb(128, 0, 128);
                    break;
                case 4:
                    color = Color.FromArgb(128, 128, 0);
                    break;
                case 5:
                    color = Color.FromArgb(0, 128, 128);
                    break;
                case 6:
                    color = Color.FromArgb(128, 128, 128);
                    break;
                case 7:
                    color = Color.FromArgb(128, 64, 64);
                    break;
                case 8:
                    color = Color.FromArgb(64, 128, 64);
                    break;
                case 9:
                    color = Color.FromArgb(64, 64, 128);
                    break;
                case 10:
                    color = Color.FromArgb(128, 64, 128);
                    break;
                case 11:
                    color = Color.FromArgb(128, 128, 64);
                    break;
                case 12:
                    color = Color.FromArgb(64, 128, 128);
                    break;
                case 13:
                    color = Color.FromArgb(128, 128, 128);
                    break;
                case 14:
                    color = Color.FromArgb(255, 64, 64);
                    break;
                case 15:
                    color = Color.FromArgb(64, 255, 64);
                    break;
                case 16:
                    color = Color.FromArgb(64, 64, 255);
                    break;
                case 17:
                    color = Color.FromArgb(255, 64, 255);
                    break;
                case 18:
                    color = Color.FromArgb(255, 255, 64);
                    break;
                default:
                    color = Color.FromArgb(64, 255, 255);
                    break;
            }

            return color;
        }
    }
}