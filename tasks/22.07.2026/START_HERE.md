# Mission Brief: Enterprise Document Rendering & PDF Generation Platform
*(Formatted for Google Antigravity — Agent Manager / Planning Mode)*

> **How to use this file in Antigravity:**
> 1. Open **Agent Manager** → select/create your workspace folder.
> 2. Start a task in **Planning Mode** (not Fast Mode) — this is a multi-week, multi-agent build, not a single-shot code-gen task.
> 3. Paste the **Mission Brief** section below as your first message.
> 4. Save the **Persistent Rules** section as `.agent/rules/document-engine.md` in the workspace so every agent/task in this project inherits it automatically.
> 5. Save the **Reusable Workflow** section as `.agent/workflows/generate-document-type.md` so you can trigger `/generate-document-type` per document category later.
> 6. Expect the agent to produce a **Task List / Implementation Plan artifact** before writing code — do not approve execution until that plan reflects the architecture below.

---

## 1. Mission Brief (paste into Agent Manager to start the task)

**Role:** Act as a Senior Enterprise Solution Architect, Lead .NET Engineer, QuestPDF Expert, API Integration Specialist, and Software Documentation Engineer.

**Objective:** Design and implement a highly scalable **Enterprise Document Rendering Platform** in .NET, capable of supporting **100+ business document types** and **1,000+ layout templates** (Invoice, Delivery Challan, Quotation, Purchase Order, Salary Slip, Certificate, and dozens more — full list in Appendix A).

This is not a one-off PDF generator. It is the foundation of a long-term document generation engine meant to evolve for years. Every decision should prioritize scalability, maintainability, modularity, reusability, performance, security, and future extensibility over short-term speed.

**Before writing any code**, enter Planning Mode and produce an **Implementation Plan artifact** covering:
1. Problem-domain analysis and scalability bottlenecks
2. Proposed software architecture with rationale
3. Folder/solution structure
4. All interfaces and abstractions
5. Reusable QuestPDF component inventory
6. Template resolution strategy
7. Rendering pipeline design
8. Configuration management approach
9. Testing strategy
10. Performance optimizations
11. Security considerations
12. Future extension points

Only after I approve the plan artifact should implementation begin. Work section by section, checking in with an updated task list after each major milestone rather than generating the whole solution in one pass.

**Core design rule:** Every document must flow through:

```
Document Type → Template → Renderer → QuestPDF Components → PDF
```

Never write type-branching logic like `if (document == "Invoice") { ... }`. That pattern is forbidden — route everything through the template resolver and renderer factory instead.

---

## 2. Persistent Rules
*(Save as `.agent/rules/document-engine.md` — Workspace Scope, so it's read by every agent on this project without repeating it per prompt.)*

```markdown
---
name: document-engine-standards
description: Standing architecture and coding rules for the Document Rendering Platform
scope: workspace
---

# Document Rendering Platform — Standing Rules

## Architecture (must not be violated)
- Layered pipeline: Controllers → Application Layer → Document Engine →
  Template Resolver → Renderer Factory → QuestPDF Renderer → Reusable
  Components → Utilities → PDF.
- No hardcoded per-document branching (no `if(document=="Invoice")`).
  All document logic routes through the Document Registry + Template
  Resolver + Renderer Factory.
- The rendering service owns NO business logic. It only forwards
  authentication, headers, query/route params, and body to the
  upstream ERP API, receives JSON, and renders. Treat it as a proxy.

## Templates & Components
- Every document type may have many templates (e.g. Invoice: Modern,
  Classic, Thermal, GST Standard, Retail). Adding a template must
  require minimal code change — no redeploy-per-template patterns.
- Layout sections (Header, Company Info, Customer Info, Address Block,
  GST Block, Items Table, Summary, Bank Details, Terms, Notes, Footer,
  QR Code, Signature, Stamp) are independent, reusable QuestPDF
  components. Never duplicate layout code across templates.
- Maintain a Document Registry mapping Document Type → Supported
  Templates → Supported Versions → Renderer → Validation Rules →
  Configuration → Permissions.

## Configuration & Security
- Nothing hardcoded: base URLs, timeouts, retry counts, storage paths,
  fonts, themes, colors, margins, default template, logo path,
  secrets, connection strings all come from config/env.
- Support Bearer Token, API Key, Cookie, JWT, and be ready for OAuth.
  Never log passwords, tokens, secrets, or private keys.
- Never expose internal exceptions to API clients — return
  descriptive, sanitized error messages only.

## Reliability
- Exponential backoff with configurable retry count and timeout
  policy for upstream calls. No infinite retries.
- Structured logging with correlation IDs, execution time, document
  type, template, request ID, rendering duration.
- Expose /health, /ready, /version endpoints.

## Coding Standards
- Clean Architecture, SOLID, DI, async/await + CancellationTokens,
  nullable reference types, strong typing, XML docs, consistent
  naming. No large monolithic classes. No duplicated code.

## PDF Output Requirements
- Support A4/A5/Letter/Thermal/custom sizes, portrait & landscape,
  repeating headers/footers, page numbering, multi-page tables with
  automatic row splitting, QR/barcodes, watermarks, digital signature
  placeholders, conditional section visibility.
- When a reference/sample PDF is supplied, match its layout, spacing,
  alignment, typography hierarchy, and table structure as closely as
  possible (minor border/color differences are acceptable).

## Testing (required, not optional)
- Unit tests: component rendering, formatting, calculations, layout
  rules, template resolution.
- Integration tests: API proxying, auth forwarding, PDF generation,
  error handling.
- Regression tests: template changes must not silently alter
  previously-shipped layouts.
- Visual/snapshot tests where feasible.

## Verification Before Marking Work Done
- Always run the solution and generate at least one real sample PDF
  per touched template before reporting a task complete. If a browser
  or file-preview tool is available, use it to visually confirm the
  rendered PDF/HTML preview rather than only checking that the build
  succeeds.
```

---

## 3. Reusable Workflow
*(Save as `.agent/workflows/generate-document-type.md`. Trigger later with `/generate-document-type` in agent chat, filling in the document name.)*

```markdown
# generate-document-type.md
Add support for a new document type to the Document Rendering Platform,
following the standing rules in document-engine-standards.

Inputs to ask the user for if not provided:
- Document type name (e.g. "Purchase Order")
- List of templates needed for it (e.g. Standard, Export, Manufacturing)
- Sample PDF or JSON payload, if available

Steps:
1. Register the document type in the Document Registry (do not branch
   in code — configuration/registry entry only).
2. Identify which existing reusable QuestPDF components cover its
   layout; only build new components for genuinely new sections.
3. Implement the template(s) via the Template Resolver + Renderer
   Factory pattern — never a new if/switch branch.
4. Add validation rules for the expected JSON payload shape.
5. Add unit tests for the new renderer/components and an integration
   test that generates a real PDF end-to-end.
6. Generate a sample PDF and, if a reference sample was provided,
   compare layout/spacing/typography against it.
7. Update API docs / Swagger / Postman collection with the new
   endpoint or payload variant.
8. Report back: what was added, which components were reused vs.
   newly created, and links to the generated sample PDF(s).
```

---

## 4. Deliverables Checklist
Ask the agent to confirm each of these exists before you consider the mission complete:

- [ ] Clean Architecture .NET solution, layered per the pipeline above
- [ ] Docker support
- [ ] README + Deployment Guide + Performance notes
- [ ] OpenAPI/Swagger docs + Postman collection
- [ ] Sample JSON payloads and sample generated PDFs (agent should
      actually render and attach these, not just describe them)
- [ ] Unit, integration, and regression tests
- [ ] Architecture diagram (ask the agent to render this as a
      diagram artifact, not just ASCII in chat)
- [ ] Implementation report summarizing what was built vs. what's
      stubbed for future document types

---

I have the schema for [Invoice / whichever document type] as a Mongoose schema (Node.js/MongoDB) — pasting below.

Important notes:
- This describes what's stored in MongoDB, not necessarily the exact
  JSON shape returned by our API. Where they differ (e.g. populated
  references, fields excluded from API responses, computed fields
  added at the API layer), I'll clarify — but use this as the source
  of truth for field names, types, and required/optional status.
- Map Mongoose `Date` → ISO 8601 string in the JSON payload.
- Map any `Decimal128` fields → treat as decimal/currency values, not
  floats.
- `ObjectId` fields will arrive as plain strings, not object references.
- [Flag any `Mixed` type fields here and describe their actual shape]

Please derive the strongly-typed C# DTO (e.g. `InvoiceDataModel`) from
this schema before implementing the first renderer, rather than
inventing field names. If anything in the schema is ambiguous or you
need me to confirm the actual API response shape for a given field,
ask before assuming.


const voucherTypes = {
    Voucher: "Voucher",
    PurchaseOrder: "Purchase Order",
    Purchase: "Purchase",
    PurchaseReturn: "Purchase Return",
    Quotation: "Quotation",
    Sale: "Sale",
    SaleReturn: "Sale Return",
    CashSale: "Cash Sale",
    CashPurchase: "Cash Purchase",
    CashPurchaseReturn: "Cash Purchase Return",
    CashSaleReturn: "Cash Sale Return",
    CashPurchaseOrder: "Cash Purchase Order",

}
const VoucherTypesConfig = {
    [voucherTypes.Voucher]: {
        name: voucherTypes.Voucher,
        prefix: "V",
        hyphenCase: "voucher"
    },
    [voucherTypes.PurchaseOrder]: {
        name: voucherTypes.PurchaseOrder,
        prefix: "PO",
        hyphenCase: "purchase-order"
    },
    [voucherTypes.Purchase]: {
        name: voucherTypes.Purchase,
        prefix: "PR",
        hyphenCase: "purchase"
    },
    [voucherTypes.PurchaseReturn]: {
        name: voucherTypes.PurchaseReturn,
        prefix: "PR",
        hyphenCase: "purchase-return"
    },
    [voucherTypes.CashPurchase]: {
        name: voucherTypes.CashPurchase,
        prefix: "CP",
        hyphenCase: "cash-purchase"
    },
    [voucherTypes.Quotation]: {
        name: voucherTypes.Quotation,
        prefix: "Q",
        hyphenCase: "quotation"
    },
    [voucherTypes.Sale]: {
        name: voucherTypes.Sale,
        prefix: "S",
        hyphenCase: "sale"
    },
    [voucherTypes.SaleReturn]: {
        name: voucherTypes.SaleReturn,
        prefix: "SR",
        hyphenCase: "sale-return"
    },
    [voucherTypes.CashSale]: {
        name: voucherTypes.CashSale,
        prefix: "CM",
        hyphenCase: "cash-memo"
    },
    [voucherTypes.CashPurchaseReturn]: {
        name: voucherTypes.CashPurchaseReturn,
        prefix: "CPR",
        hyphenCase: "cash-purchase-return"
    },
    [voucherTypes.CashSaleReturn]: {
        name: voucherTypes.CashSaleReturn,
        prefix: "CSR",
        hyphenCase: "cash-sale-return"
    },
    [voucherTypes.CashPurchaseOrder]: {
        name: voucherTypes.CashPurchaseOrder,
        prefix: "CPO",
        hyphenCase: "cash-purchase-order"
    },
}
const VoucherStatusTypes = {
    Draft: "Draft",
    Pending: "Pending",
    Approved: "Approved",
    Rejected: "Rejected",
}
//   ["REFUND"]: "bg-orange-100 text-orange-600",
//   ["SENT"]: "bg-green-100 text-green-600",
//   ["UNPAID"]: "bg-orange-100 text-orange-600",
//   ["PARTIALLY_PAID"]: "bg-orange-100 text-orange-600",
//   ["CANCELLED"]: "bg-red-100 text-red-600",
//   ["OVERDUE"]: "bg-red-100 text-red-600",
//   ["PAID"]: "bg-green-100 text-green-600",
//   ["DRAFTED"]: "bg-orange-100 text-orange-500",
const InvoiceStatusTypes = {
    Draft: "Draft",
    Approved: "Approved",
    Sent: "Sent",
    Unpaid: "Unpaid",
    PartiallyPaid: "Partially Paid",
    Overdue: "Overdue",
    Paid: "Paid",
    Cancelled: "Cancelled",
    RefundRequested: "Refund Requested",
    RefundProcessed: "Refund Processed",
}
const VoucherTypesArray = Object.values(voucherTypes);
const VoucherStatusTypesArray = Object.values(VoucherStatusTypes);
const InvoiceStatusTypesArray = Object.values(InvoiceStatusTypes);
module.exports = {
    voucherTypes,
    VoucherTypesArray,
    InvoiceStatusTypes,
    VoucherTypesConfig,
    VoucherStatusTypes,
    InvoiceStatusTypesArray,
    VoucherStatusTypesArray
}


const mongoose = require("mongoose");
const PaymentMethods = require("../../data/PaymentMethods");
const { Schema } = mongoose;
const { InvoiceStatusTypes, VoucherStatusTypes, VoucherTypesArray, voucherTypes, InvoiceStatusTypesArray, VoucherStatusTypesArray } = require("./voucher");
const VoucherSchema = new Schema(
  {
    number: {
      type: String,
      required: true,
      trim: true,
      name: "Voucher Number",
    },
    party: {
      type: Schema.Types.ObjectId,
      ref: "Party",
      required: true,
      name: "Party",
    },
    date: {
      type: Date,
      required: true,
      name: "Date",
    },
    dueDate: {
      type: Date,
      required: true,
      name: "Due Date",
    },
    referenceNumber: {
      type: String,
      trim: true,
      name: "Reference Number",
    },
    paymentMethod: {
      type: String,
      enum: Object.values(PaymentMethods),
      name: "Payment Method",
    },
    address: {
      type: String,
      trim: true,
      name: "Address",
    },
    products: [
      {
        product: {
          type: Schema.Types.ObjectId,
          ref: "Product",
          required: true,
          name: "Product",
        },
        name: {
          type: String,
          trim: true,
          name: "Product Name",
        },
        hsn: {
          type: String,
          trim: true,
          name: "HSN/SAC Code",
        },
        quantity: {
          type: Number,
          required: true,
          min: 0,
          name: "Quantity",
        },
        price: {
          type: Number,
          min: 0,
          name: "Price",
        },
        unit: {
          type: Schema.Types.ObjectId,
          ref: "Unit",
          name: "Unit",
        },
        discountType: {
          type: String,
          enum: ["fixed", "percentage"],
          default: "fixed",
          name: "Discount Type",
        },
        discountValue: {
          type: Number,
          min: 0,
          default: 0,
          name: "Discount Value",
        },
        discountAmount: {
          type: Number,
          min: 0,
          default: 0,
          name: "Discount Amount",
        },
        // Taxes
        taxes: [
          {
            name: {
              type: String,
              trim: true,
            },
            value: {
              type: Number,
            },
          }
        ],
        taxAmount: {
          type: Number,
          min: 0,
          default: 0,
          name: "Tax Amount",
        },
        taxableAmount: {
          type: Number,
          min: 0,
          default: 0,
          name: "Taxable Amount",
        },
        amount: {
          type: Number,
          min: 0,
          default: 0,
          name: "Amount",
        }
      },
    ],
    bank: {
      type: Schema.Types.ObjectId,
      ref: "Bank",
      name: "Bank",
    },
    notes: {
      type: String,
      trim: true,
      name: "Notes",
    },
    terms: {
      type: String,
      trim: true,
      name: "Terms",
    },
    cost: [
      {
        name: {
          type: String,
          required: true,
          trim: true,
          name: "Cost Name",
        },
        value: {
          type: Number,
          required: true,
          min: 0,
          name: "Cost Value",
        },
      },
    ],
    discountType: {
      type: String,
      enum: ["fixed", "percentage"],
      default: "fixed",
      name: "Discount Type",
    },
    discountValue: {
      type: Number,
      min: 0,
      default: 0,
      name: "Discount Value",
    },
    couponCode: {
      type: String,
      trim: true,
      name: "Coupon Code",
    },
    signature: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "Signature",
      trim: true,
      name: "Signature",
    },
    status: {
      type: String,
      enum: VoucherStatusTypesArray,
      default: "pending",
      name: "Status",
    },
    paymentStatus: {
      type: String,
      enum: InvoiceStatusTypesArray,
      default: "unpaid",
      name: "Payment Status",
    },
    // Computed/Summary Fields for Indian Invoice Requirements
    subtotal: {
      type: Number,
      min: 0,
      default: 0,
      name: "Subtotal",
    },
    totalDiscountAmount: {
      type: Number,
      min: 0,
      default: 0,
      name: "Total Discount",
    },
    totalTaxAmount: {
      type: Number,
      min: 0,
      default: 0,
      name: "Total Tax",
    },
    totalCostAmount: {
      type: Number,
      min: 0,
      default: 0,
      name: "Total Additional Cost",
    },
    grandTotal: {
      type: Number,
      min: 0,
      default: 0,
      name: "Grand Total",
    },
    roundOff: {
      type: Number,
      default: 0,
      name: "Round Off",
    },
    amountInWords: {
      type: String,
      trim: true,
      name: "Amount in Words",
    },
    type: {
      type: String,
      enum: VoucherTypesArray,
      default: voucherTypes.Voucher,
      name: "Type",
    },
    user: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
      name: "User",
    },
    nextVoucher: {
      type: Schema.Types.ObjectId,
      ref: "Voucher",
      name: "Next Voucher",
    },
    previousVoucher: {
      type: Schema.Types.ObjectId,
      ref: "Voucher",
      name: "Previous Voucher",
    },
    nextMaterialEntry: {
      type: Schema.Types.ObjectId,
      ref: "MaterialEntry",
      name: "Next Material Entry",
    },
    previousMaterialEntry: {
      type: Schema.Types.ObjectId,
      ref: "MaterialEntry",
      name: "Previous Material Entry",
    },
    isDeleted: {
      type: Boolean,
      default: false,
      name: "Is Deleted",
    },
  },
  {
    timestamps: true,
  }
);
const Voucher = mongoose.model("Voucher", VoucherSchema);
module.exports = Voucher;




## Appendix A — Initial Document Type List
### Voucher Types

* Voucher
* Purchase Order
* Purchase
* Purchase Return
* Quotation
* Sale
* Sale Return
* Cash Sale
* Cash Purchase

### Material Entry Types

* Material Entry
* Material Issue
* Material Requisition
* Purchase Requisition
* Goods Receipt Note (GRN)
* Finished Goods Receipt Note (FGRN)
* QC Entry
* QC Rejection Entry
* Scrap Entry
* By-Product Entry
* Rework Entry
* Sample Issue Entry
* Damage Entry
* Expiry Write-Off Entry
* Delivery Challan

### Other Document Types

* Stock Transfer In
* Stock Transfer Out



## Appendix B — Future Extensibility Targets
HTML rendering, Excel generation, Word generation, image generation, email/SMS/WhatsApp templates, label/thermal printing, multi-language + RTL support, theme engine, per-tenant branding, multi-tenancy, versioned templates, template marketplace, cloud storage integrations, digital signature providers, PDF/A compliance, batch generation, background rendering queues, scheduled generation.