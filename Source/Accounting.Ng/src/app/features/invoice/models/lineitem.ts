export interface ILineitem {
    id: number,
    name: string,
    description: string,
    quantity: number,
    amount: number,
    gst: number,
    total: number,
    isZeroRated: boolean,
    invoiceId: number
}