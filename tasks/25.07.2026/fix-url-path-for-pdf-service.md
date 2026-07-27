**Task: Fix PDF service render URL routing and consolidate voucher templates**

**1. Fix the render route**

Current URL structure is wrong. Update it to:

```
GET /render/<model>/:id
```

Where `<model>` is one of:
- `voucher`
- `material-entry`
- `work-order`r

No `v1`, no `api` prefix. Final URL example:

```
http://localhost:5029/render/voucher/6a61be54cfc2809fe2131bfa
```

**2. Render controller behavior**

The render controller must call the upstream service's print route to fetch the correct template from settings (not resolve the template itself).

**3. Template resolution logic**

For the `voucher` model, resolve the template name from the Voucher Settings schema field:

```js
pdf_template: {
  type: String,
  trim: true,
  maxlength: 100,
  required: true,
  name: "PDF Template",
  default: "Standard"
}
```

- `"Standard"` is always the default template name.
- Use `pdf_template` value from voucher settings to select which renderer to invoke.

**4. Template cleanup**

- Delete: `pdf-service/src/NextWeb.DocumentPlatform.Renderers/Templates/VoucherStandardRenderer.cs` (unused).
- Rename: `pdf-service/src/NextWeb.DocumentPlatform.Renderers/Templates/GstStandardRenderer.cs` → becomes the new **Standard** renderer for the `voucher` document type (i.e., this file/class now serves as `VoucherStandardRenderer` / the "Standard" template for vouchers).

**5. Make sure every DTO is exact to the model described in the models directory**

- Read the models/<model>.md for making DTO perfect and complete in one go. 
- Check if there are any missing fields and add them to the DTO
- Do not remove any field from the DTO as it may come from the API response. 
- I also have added API response sample in `pdf-service\context\QUEST PDF.json` file. 


**6. Acceptance criteria**

- Hitting `/render/voucher/:id` resolves the settings' `pdf_template` field, defaults to `"Standard"` if unset, and renders using the (renamed) GST-based standard renderer.
- Old `VoucherStandardRenderer.cs` is removed with no dangling references.
- `/render/material-entry/:id` and `/render/work-order/:id` follow the same route pattern (no v1/api prefix).



