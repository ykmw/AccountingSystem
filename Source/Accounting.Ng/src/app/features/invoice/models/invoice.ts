export interface IInvoice {
    id: number,
    invoiceRef: string,
    purchaseOrder: string,
    date: string,
    isGSTExclusive: boolean,
    subTotal: number,
    gst: number,
    discount: number,
    total: number,
    status: number,
    lineItem: [
        {
            id: number,
            name: string,
            description: string,
            quantity: number,
            amount: number,
            gst: number,
            total: number,
            isZeroRated: true,
            invoiceId: number
        }
    ]
}