### Abandonware

Essentially this sits in the request pipeline, intercepting HTTP calls
and providing hooks to do authentication outside of IIS.  This allows
for implementing basic / digest auth easily without involving IIS and
it's dependency on being tied to Active Directory, rather than to a
database, which is the usual implementation.

At one point this was a viable security infrastructure that
was intended to be cleaned up and open-sourced.

The code should still work fine, and work well -- but it will
require a bit of polish to ensure it's well tested and can be officially
released on NuGet.

If you're still working in a Microsoft world, and haven't moved on to
something like ServiceStack and it's simple plugin architecture, then
this might be a good fit for you.

There are a handful of nifty tricks around mocking, and providing
lightweight handlers for Authentication, based strictly on `Func<>`.

At one point this code used a Git submodule for build tooling -- that
has been removed, but the projects haven't been properly reconstructed
or setup with a tool like Psake -- that work would need to be performed
before this can even build.
