GOAL: Implement the "Tax Invoice / e-Invoice" template using the attached 
reference image as the exact visual target, and confirm the payload shape 
against the print endpoint before building.

## Reference materials attached
1. `Instant-e-invoice-in-TallyPrime.webp` — this is the TARGET LAYOUT. 
   Treat every visible field/section as a required element to reproduce.
2. `QUEST_PDF.json` — an OpenAPI file describing our print endpoint 
   (GET /api/voucher/:id/print, bearer auth). 

   IMPORTANT: this file only describes the ROUTE — it has no response 
   schema/example body. Do NOT infer field names from it. Before 
   building the DTO for this template, I need to either:
   (a) call this endpoint myself with a real token and paste you the 
       actual JSON response, or
   (b) you tell me exactly what's still missing so I know what to capture.
   
   Wait for the real response payload before finalizing field names in 
   the InvoiceDataModel — do not guess based on the image alone.

## Fields/sections visible in the reference image — enumerate and confirm 
each has a home in the payload/DTO before rendering:

**Header block**
- Document title: "Tax Invoice" (left) + "e-Invoice" label (right)
- QR code (top right) — this is GST e-invoice QR data, NOT our existing 
  generic QrCodeComponent use case; confirm what this QR should encode 
  (typically IRN + invoice details per GST spec) — flag if we don't 
  have this data yet.

**e-Invoice compliance block (NEW — not in original component list)**
- IRN (Invoice Reference Number) — long hash string
- Ack No. (Acknowledgement Number)
- Ack Date

**Document meta block**
- Invoice No.
- Dated
- Delivery Note / Delivery Note Date
- Mode/Terms of Payment
- Reference No. & Date
- Other References
- Buyer's Order No. / Dated
- Dispatch Doc No.
- Dispatched through / Destination
- Terms of Delivery

**Three-way party block (NEW — this is more than our current Party concept)**
- Seller block: business name, address, GSTIN/UIN, State Name + Code
- Consignee (Ship to): name, address, GSTIN/UIN, State Name + Code
- Buyer (Bill to): name, address, GSTIN/UIN, State Name + Code

  Confirm: does our voucher model distinguish Consignee (ship-to) from 
  Buyer (bill-to) as separate parties, or do we currently only have one 
  "Party" reference? This is a real gap if we only have one — GST 
  invoices legally require both to be shown even when they're the same 
  entity.

**Line items table**
- SI No., Description of Goods, HSN/SAC code, Quantity, Rate, per (unit), 
  Disc. %, Amount
- Below each line item row: CGST/SGST amount lines (or IGST for 
  interstate — check if the reference always shows CGST+SGST or if 
  IGST is a separate template variant for interstate sales)
- Total row: total quantity + total amount

**Amount in words**
- "Amount Chargeable (in words)" — confirm this reads currency name 
  correctly per our earlier currency work (image shows "Indian Rupee 
  Four Thousand One Hundred Thirty Only" — confirm this component 
  already produces this exact phrasing pattern, or needs adjustment)

**Tax breakdown table (separate from line items table)**
- HSN/SAC, Taxable Value, Central Tax (Rate + Amount), State Tax 
  (Rate + Amount), Total Tax Amount — with a Total row
- "Tax Amount (in words)" — same amount-in-words logic, applied to the 
  tax total specifically, not the grand total

**Footer**
- Declaration text (standard boilerplate — confirm if this should be 
  configurable per business/template or hardcoded per GST convention)
- "for [Business Name]" + "Authorised Signatory" block (right-aligned, 
  no actual signature/stamp in this reference — treat as a text-only 
  placeholder unless we have signature image support already)
- "This is a Computer Generated Invoice" footer line

## Instructions
1. Do NOT start building until I've provided the real API response 
   payload for a test voucher (per point above).
2. Once you have it, map every field enumerated above to a specific 
   JSON path in that payload. List back to me any field from the image 
   that has NO corresponding data in the payload — those are gaps we 
   need to close in the print controller or voucher model before this 
   template can be considered complete.
3. Flag explicitly whether IRN/Ack No./Ack Date/e-invoice QR data are 
   things we currently generate/store at all, or if this is a new 
   feature requirement (GST e-invoicing is a specific government 
   compliance system — this may need its own integration, not just a 
   rendering fix).
4. This appears to be an India-specific GST tax invoice template 
   ("GST Standard" style, referenced early in our original template 
   list). Confirm with me whether this is meant to be the default 
   "Standard" template for all invoices, or a specific GST-compliant 
   variant that should be selectable separately (e.g. for domestic 
   B2B sales only, not for all voucher types).

Wait for my response before writing any renderer or DTO code for this 
template.