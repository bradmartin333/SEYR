namespace SEYRHelper

open System.IO

module Report =
    type Entry = { ImageNumber:int;
                   Row:int;
                   Col:int;
                   Name:string;
                   Verdict:string; }
    
    let toEntry (data:string) =
        let columns = data.Split('\t')
        let ImageNumber = int columns.[0]  
        let Row = int columns.[1]
        let Col = int columns.[2]
        let Name = string columns.[3]
        let Verdict = string columns.[4]
        { ImageNumber = ImageNumber;
          Name = Name;
          Row = Row;
          Col = Col;
          Verdict = Verdict }

    let reader (path:string) =
        let data = File.ReadAllLines path
        data
        |> Array.filter(fun x -> x <> "")
        |> Array.map toEntry

    let data (path:string) = reader path

    let getNumImages (data:Entry[]) =
        data
        |> Array.map(fun x -> x.ImageNumber)
        |> Array.toSeq
        |> Seq.distinct
        |> Seq.length

    let getNumX (data:Entry[]) =
        data
        |> Array.map(fun x -> x.Row)
        |> Array.toSeq
        |> Seq.distinct
        |> Seq.length

    let getNumY (data:Entry[]) =
        data
        |> Array.map(fun x -> x.Col)
        |> Array.toSeq
        |> Seq.distinct
        |> Seq.length

    let getImage (data:Entry[], num:int) =
        data
        |> Array.filter(fun x -> x.ImageNumber = num)

    let getCell (data:Entry[], row:int, col:int) =
        data
        |> Array.filter(fun x -> x.Row = row && x.Col = col)